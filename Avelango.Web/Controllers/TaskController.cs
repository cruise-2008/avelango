using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Avelango.Handlers.File;
using Avelango.Hubs.Accessory;
using Avelango.Models;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ILog _log;
        private readonly ITasks _task;
        private readonly IChats _chats;
        private readonly ITaskBids _taskBids;
        private readonly ITaskOffers _taskOffers;
        private readonly INotifycations _notifycations;
        private readonly ITaskPreWorkers _taskPreWorkers;
        private readonly ITaskAttachments _taskAttachments;

        public TaskController(ITasks task, ITaskAttachments taskAttachments, ITaskBids taskBids, ITaskPreWorkers taskPreWorkers, ILog log, ITaskOffers taskOffers, INotifycations notifycations, IChats chats) {
            _task = task;
            _taskAttachments = taskAttachments;
            _taskBids = taskBids;
            _taskPreWorkers = taskPreWorkers;
            _log = log;
            _taskOffers = taskOffers;
            _notifycations = notifycations;
            _chats = chats;
        }


        // GET: /Task/Tasks
        public ActionResult Tasks(string subg, Guid? pk, int? prop) {
            try {
                if (prop == 1) {
                    var propPks = _notifycations.GetMyProposals(new PrivateSession().Current.User.Pk).Data;
                    if (propPks.Any()) new PrivateSession().Current.ProposalsTaskPks = propPks;
                }
                ViewBag.PublicKey = pk;
                ViewBag.Subgroup = subg;
                return View();
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[Tasks]", ex.Message);
                return View();
            }
        }


        public ActionResult TaskCard()
        {
            return View();
        }


        // POST: /Task/GetTask
        [HttpPost]
        public ActionResult GetTask(Guid pk) {
            try {
                var user = new PrivateSession().Current.User;
                var tasksResult = _task.GetTaskByPk(user?.Pk ?? Guid.Empty, pk);
                if (!tasksResult.IsSuccess) return Json(new { IsSuccess = false });
                return Json(new { IsSuccess = true, Job = new List<ApplicationTask> {tasksResult.Data} });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[GetTask]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /Task/GetFilteredTasks
        [HttpPost]
        public ActionResult GetFilteredTasks(List<Guid> viewedTasks, List<string> subgroups, bool justActual, double? placeLat, double? placeLng, int countOfTasks) {
            try {
                var current = new PrivateSession().Current;
                if (new PrivateSession().Current.ProposalsTaskPks != null) {
                    var tasksResult = _task.GetTasksByPks(current.User.Pk, current.ProposalsTaskPks);
                    new PrivateSession().Current.ProposalsTaskPks = null;
                    return !tasksResult.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true, Jobs = tasksResult.Data });
                }
                else {
                    viewedTasks = viewedTasks ?? new List<Guid>();
                    subgroups = subgroups ?? new List<string>();
                    var tasksResult = _task.GetFilteredTasks(current.User?.Pk ?? Guid.Empty, viewedTasks, subgroups, justActual, countOfTasks, placeLat, placeLng);
                    return !tasksResult.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true, Jobs = tasksResult.Data });
                }
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[GetFilteredTasks]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /Task/AddTask
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult AddTask(string name, string description, string price, string placeLat, string placeLng, string placeName, string topicaltoData, string topicaltoHours, List<HttpPostedFileBase> files) {
            try {
                name = new JavaScriptSerializer().Deserialize<string>(name);
                description = new JavaScriptSerializer().Deserialize<string>(description);
                price = new JavaScriptSerializer().Deserialize<string>(price);
                placeLat = new JavaScriptSerializer().Deserialize<string>(placeLat);
                placeLng = new JavaScriptSerializer().Deserialize<string>(placeLng);
                placeName = new JavaScriptSerializer().Deserialize<string>(placeName);
                topicaltoData = new JavaScriptSerializer().Deserialize<string>(topicaltoData);
                topicaltoHours = new JavaScriptSerializer().Deserialize<string>(topicaltoHours);

                var userPk = new PrivateSession().Current.User.Id;
                DateTime dtTopicalto;
                DateTime.TryParse(Regex.Match(topicaltoData, @"\d{4}-\d{2}-\d{2}").Value + " " + topicaltoHours.Replace('-', ':'), out dtTopicalto);

                var operationResult = _task.AddTasks(name, description, price, placeName, Convert.ToDouble(placeLat), Convert.ToDouble(placeLng), dtTopicalto, userPk);

                var savedFiles = new OperationResult<List<ApplicationTaskAttachment>>();
                var saveResult = new OperationResult<bool>();
                if (files != null && files.Any()) {
                    savedFiles = FileManager.SaveTaskAttachments(new PrivateSession().Current.CurrentLang, Server.MapPath("~"), files.ToList());
                    saveResult = _taskAttachments.Add(savedFiles.Data, operationResult.Data);
                }
                // Send broadcust to group
                if (operationResult.IsSuccess) {
                    HubClient.NewTaskAddedForModerator(operationResult.Data.ToString());
                    HubClient.NewTaskAddedForCustomer(new PrivateSession().Current.User.Pk.ToString(), operationResult.Data.ToString());
                }
                var exceptions = new List<Exception> {operationResult.Exception, savedFiles.Exception, saveResult.Exception};
                return Json(new { IsSuccess = operationResult.IsSuccess, Exeptions = exceptions});
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[AddTask]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderEdit
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrderEdit(string taskPk, string name, string description, string price, string placeName, string placeLat, string placeLng,
                                        string topicaltoData, string topicaltoHours, List<string> removedAttachmentsPathes, List<HttpPostedFileBase> files) {
            try {
                Guid guidTaskPk;
                var desTaskPk = new JavaScriptSerializer().Deserialize<string>(taskPk);
                Guid.TryParse(desTaskPk, out guidTaskPk);
                if (guidTaskPk == Guid.Empty) return Json(new {IsSuccess = false});
                name = new JavaScriptSerializer().Deserialize<string>(name);
                description = new JavaScriptSerializer().Deserialize<string>(description);
                price = new JavaScriptSerializer().Deserialize<string>(price);
                placeName = new JavaScriptSerializer().Deserialize<string>(placeName);
                placeLat = new JavaScriptSerializer().Deserialize<string>(placeLat);
                placeLng = new JavaScriptSerializer().Deserialize<string>(placeLng);


                topicaltoData = new JavaScriptSerializer().Deserialize<string>(topicaltoData);
                topicaltoHours = new JavaScriptSerializer().Deserialize<string>(topicaltoHours);
                DateTime dtTopicalto;
                DateTime.TryParse(Regex.Match(topicaltoData, @"\d{4}-\d{2}-\d{2}").Value + " " + topicaltoHours.Replace('-', ':'), out dtTopicalto);

                // remove non-used attachments
                if (removedAttachmentsPathes != null && removedAttachmentsPathes.Any()) {
                    for (var i = 0; i < removedAttachmentsPathes.Count; i++) {
                        removedAttachmentsPathes[i] = removedAttachmentsPathes[i].Replace("..\\", Server.MapPath("~"));
                    }
                    var listFileNamesRemoved = FileManager.RemoveTaskAttachments(removedAttachmentsPathes).Data;
                    if (listFileNamesRemoved.Any()) {
                        _taskAttachments.Remove(listFileNamesRemoved, guidTaskPk);
                    }
                }
                // add files as attachments to Db
                if (files != null && files.Any()) {
                    var savedFiles = FileManager.SaveTaskAttachments(new PrivateSession().Current.CurrentLang, Server.MapPath("~"), files.ToList());
                    _taskAttachments.Add(savedFiles.Data, guidTaskPk);
                }

                var parsedPrice = 0;
                int.TryParse(price, out parsedPrice);
                var taskEdited = _task.MyOrderEdit(new PrivateSession().Current.User.Pk, guidTaskPk, name, description, parsedPrice, placeName, 
                                                   Convert.ToDouble(placeLat), Convert.ToDouble(placeLng), dtTopicalto);

                if (taskEdited.IsSuccess) {
                    HubClient.NewTaskAddedForModerator(new JavaScriptSerializer().Serialize(new {
                        taskPk = desTaskPk,
                        title = name
                    }));
                    HubClient.NewTaskAddedForCustomer(new PrivateSession().Current.User.Pk.ToString(), desTaskPk);
                }
                return Json(!taskEdited.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderEdit]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /Task/ConfirmTask
        //[HttpPost]
        //[AccessLevelModerator]
        //public ActionResult ConfirmTask(string id, string group, string subgroup) {
        //    try {
        //        Guid userPublicKey;
        //        Guid.TryParse(id, out userPublicKey);
        //        if (userPublicKey == Guid.Empty) return Json(false);
        //
        //        var task = _task.ConfirmTask(userPublicKey, group, subgroup);
        //        var moderPk = new PrivateSession().Current.User.Pk;
        //
        //        // Send broadcust to group
        //        if (!task.IsSuccess) return Json(new { IsSuccess = false });
        //        var jsonObj = new JavaScriptSerializer().Serialize(new {
        //            name = task.Data.Name,
        //            description = task.Data.Description,
        //            price = task.Data.Price,
        //            topicalto = task.Data.TopicalTo,
        //            created = task.Data.Created,
        //            group = task.Data.Group,
        //            subGroup = task.Data.SubGroup,
        //            customer = new {
        //                name = task.Data.Customer.Name,
        //                rating = task.Data.Customer.Rating,
        //                logo = task.Data.Customer.UserLogoPath,
        //            }
        //        });
        //        HubClient.NewTaskConfirmed(task.Data.SubGroup, jsonObj);
        //        return Json(new { IsSuccess = true });
        //    }
        //    catch (Exception ex) {
        //        _log.AddError("[Task]/[ConfirmTask]", ex.Message);
        //        return Json(new { IsSuccess = false });
        //    }
        //}



        // POST: /Task/BidTask
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult BidTask(Guid taskPk, string message, int price) {
            try {
                var user = new PrivateSession().Current.User;
                var bidTaskResult = _taskBids.BidTask(taskPk, user.Pk, message, price);
                if (bidTaskResult.IsSuccess) {
                    _chats.AddChat(taskPk, user.Pk);
                    _notifycations.AddNotification(NotifycationTypes.TaskBidded.ToString(), taskPk, Guid.Parse(bidTaskResult.Data.Customer.Pk), user.Pk);
                    HubClient.TaskBidded(bidTaskResult.Data.Customer.Pk, new JavaScriptSerializer().Serialize(new {
                        title = bidTaskResult.Data.Name,
                        workerName = user.Name,
                        taskPk = bidTaskResult.Data.PublicKey
                    }));
                }
                return Json(bidTaskResult.IsSuccess ? new { IsSuccess = true } : new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[BidTask]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }



        // POST: /Task/RemoveBid
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult RemoveBid(Guid taskPk) {
            try {
                var user = new PrivateSession().Current.User;
                var bidRemovedResult = _taskBids.RemoveBid(taskPk, user.Pk);
                if (bidRemovedResult.IsSuccess) {
                    HubClient.TaskUnbidded(bidRemovedResult.Data.Customer.Pk, new JavaScriptSerializer().Serialize(new {
                        title = bidRemovedResult.Data.Name,
                        workerName = user.Name,
                        taskPk = bidRemovedResult.Data.PublicKey
                    }));
                }
                return Json(bidRemovedResult.IsSuccess ? new { IsSuccess = true } : new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[RemoveBid]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }



        // /Task/GetMyOrders
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult GetMyOrders() {
            try {
                var orders = _task.GetMyOrders(new PrivateSession().Current.User.Pk);
                return !orders.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true, Orders = orders.Data });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[RemoveBid]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderRemove
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrderRemove(Guid taskPk) {
            try {
                var atask = _task.MyOrderRemove(new PrivateSession().Current.User.Pk, taskPk);
                if (atask.IsSuccess && atask.Data != null) {
                    HubClient.TaskUnbidded(atask.Data.Customer.Pk, new JavaScriptSerializer().Serialize(new {
                        title = atask.Data.Name,
                        workerName = new PrivateSession().Current.User.Name,
                        taskPk = atask.Data.PublicKey
                    }));
                }
                return Json(!atask.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderRemove]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrdersRemove
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrdersRemove(List<Guid> tasksPk) {
            try {
                foreach (var taskPk in tasksPk) {
                    _task.MyOrderRemove(new PrivateSession().Current.User.Pk, taskPk);
                }
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderRemove]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderReopen
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrderReopen(Guid taskPk) {
            try {
                var orders = _task.MyOrderReopen(new PrivateSession().Current.User.Pk, taskPk);
                return Json(!orders.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderReopen]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderClose
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrderClose(Guid taskPk, int rate, string mention) {
            try {
                var userPk = new PrivateSession().Current.User.Pk;
                var orderCloseResult = _task.MyOrderClose(userPk, taskPk);
                if (!orderCloseResult.IsSuccess)
                    return orderCloseResult.IsSuccess
                        ? Json(new {IsSuccess = true, Status = orderCloseResult.Data})
                        : Json(new {IsSuccess = false});

                if (!orderCloseResult.Data.ClosedByCustomer) {
                    _notifycations.AddNotification(NotifycationTypes.TaskCompletedByWorker.ToString(), taskPk, Guid.Parse(orderCloseResult.Data.Customer.Pk), userPk);
                    HubClient.TaskCompletedByWorker(orderCloseResult.Data.Customer.Pk,
                        new JavaScriptSerializer().Serialize(new {
                            taskPk = orderCloseResult.Data.PublicKey,
                            title = orderCloseResult.Data.Name }));
                }
                else {
                    _notifycations.AddNotification(NotifycationTypes.TaskCompletedByCustomer.ToString(), taskPk, orderCloseResult.Data.Worker.PublicKey, userPk);
                    HubClient.TaskCompletedByCustomer(orderCloseResult.Data.Worker.PublicKey.ToString(),
                        new JavaScriptSerializer().Serialize(new {
                            taskPk = orderCloseResult.Data.PublicKey,
                            title = orderCloseResult.Data.Name }));
                }
                return orderCloseResult.IsSuccess ? Json(new { IsSuccess = true, Status = orderCloseResult.Data.ProccessStatus }) : Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderClose]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderToDisput
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyOrderToDisput(Guid taskPk) {
            try {
                var orders = _task.MyOrderToDisput(new PrivateSession().Current.User.Pk, taskPk);
                return Json(!orders.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyOrderToDisput]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/GetMyJobs
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult GetMyJobs() {
            try {
                var jobsResult = _task.GetMyJobs(new PrivateSession().Current.User.Pk);
                return jobsResult.IsSuccess ? Json(new { IsSuccess = true, Jobs = jobsResult.Data }) : Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[GetMyJobs]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/MyOrderToDisput
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult MyJob(string pk) {
            try {
                Guid taskPk;
                Guid.TryParse(pk, out taskPk);
                if (taskPk == Guid.Empty) { return Json(new { IsSuccess = false }); }

                var orders = _task.MyOrderToDisput(new PrivateSession().Current.User.Pk, taskPk);
                return Json(!orders.IsSuccess ? new { IsSuccess = false } : new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[MyJob]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/GetTaskBiders
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult GetTaskBiders(Guid taskPk) {
            try {
                var taskBiders = _taskBids.GetTaskBiders(new PrivateSession().Current.User.Pk, taskPk);
                return !taskBiders.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true, TaskBiders = taskBiders.Data });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[GetTaskBiders]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/SetExecutors
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult SetTaskPreWorkers(Guid taskPk, List<Guid> executorsPks) {
            try {
                var userPk = new PrivateSession().Current.User.Pk;
                var setResult = _taskPreWorkers.SetTaskPreWorkers(userPk, taskPk, executorsPks);
                if (setResult.IsSuccess) {
                    var executorsPk = executorsPks.Select(executorPk => executorPk.ToString()).ToList();
                    _notifycations.AddNotifications(NotifycationTypes.CustomerChosenTheWorkers.ToString(), taskPk, executorsPks, userPk);
                    HubClient.CustomerChoseWorkers(executorsPk, 
                        new JavaScriptSerializer().Serialize(new {
                            taskPk = setResult.Data.PublicKey,
                            title = setResult.Data.Name}));
                }
                return !setResult.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[SetTaskPreWorkers]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/TaskStartWorking
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult TaskStartWorking(Guid taskPk) {
            try {
                var userPk = new PrivateSession().Current.User.Pk;
                var taskStartWorkingResult = _task.TaskStartWorking(userPk, taskPk);
                if (taskStartWorkingResult.IsSuccess) {
                    _notifycations.AddNotification(NotifycationTypes.WorkerStartedTask.ToString(), taskPk, Guid.Parse(taskStartWorkingResult.Data.Customer.Pk), userPk);
                    HubClient.WorkerStartedTask(taskStartWorkingResult.Data.Customer.Pk, new JavaScriptSerializer().
                        Serialize(new {
                            taskPk = taskStartWorkingResult.Data.PublicKey,
                            title = taskStartWorkingResult.Data.Name,
                            workerName = taskStartWorkingResult.Data.Worker.Name
                        }));

                    var listOfLoosers = taskStartWorkingResult.Data.Biders.Select(i4 => i4.PublicKey.ToString()).ToList();
                    HubClient.WorkersIsLateToGetTask(listOfLoosers, new JavaScriptSerializer().Serialize(new { taskPk = taskPk }));
                }
                return !taskStartWorkingResult.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[TaskStartWorking]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /Task/TaskStartWorking
        [HttpPost]
        [AccessLevelAnyAutorized]
        public ActionResult SetOffers(List<Guid> tasksPk, Guid toUser, string message) {
            try {
                var user = new PrivateSession().Current.User;
                var offersSetted = _taskOffers.SetOffers(user.Pk, tasksPk, message);
                _notifycations.AddNotifications(NotifycationTypes.ProposalOfTask.ToString(), tasksPk, toUser, user.Pk);
                foreach (var taskPk in tasksPk) {
                    HubClient.WorkerHasGotProposition(toUser.ToString(), new JavaScriptSerializer().Serialize(new {
                        taskPk = taskPk,
                        customerName = user.Name
                    }));
                }
                return !offersSetted.IsSuccess ? Json(new { IsSuccess = false }) : Json(new { IsSuccess = true });
            }
            catch (Exception ex) {
                _log.AddError("[Task]/[TaskStartWorking]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }
        
    }
}