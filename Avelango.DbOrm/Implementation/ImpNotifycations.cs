using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Orm;
using Avelango.Models.Enums;


namespace Avelango.DbOrm.Implementation
{
    public class ImpNotifycations : INotifycations
    {
        private readonly IRepository<Notifications> _notifications;
        private readonly IRepository<Users> _users;
        private readonly IRepository<Tasks> _tasks;

        public ImpNotifycations(IRepository<Tasks> tasks, IRepository<Users> users, IRepository<Notifications> notifications) {
            _notifications = notifications;
            _tasks = tasks;
            _users = users;
        }



        public OperationResult<bool> AddNotification(string notifyType, Guid taskPk, Guid userPk, Guid fromUserPk) {
            try {
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                var colocutor = _users.GetSingleOrDefault(x => x.PublicKey == fromUserPk);
                if (task == null || user == null || colocutor == null ) return new OperationResult<bool>(new Exception("AddNotification: task, user or colocuter doesn't found"));

                if (_notifications.GetFirstOrDefault(x => x.BelongsToTask == task.ID && x.NotificationType == notifyType &&
                                        x.BelongsToUser == user.ID && x.BelongsToFromUser == colocutor.ID) != null) return new OperationResult<bool>();
                _notifications.Add(new Notifications {
                    Created = DateTime.Now,
                    NotificationType = notifyType,
                    PublicKey = Guid.NewGuid(),
                    Reviewed = false,
                    BelongsToTask = task.ID,
                    BelongsToUser = user.ID,
                    BelongsToFromUser = colocutor.ID
                });
                _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<bool> AddNotifications(string notifyType, List<Guid> tasksPk, Guid userPk, Guid fromUserPk) {
            try {
                var hasChanges = false;
                var colocutor = _users.GetSingleOrDefault(x => x.PublicKey == fromUserPk);

                foreach (var taskPk in tasksPk) {
                    var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                    var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                    if (task == null || user == null || colocutor == null) continue;
                    if (_notifications.GetFirstOrDefault(x => x.BelongsToTask == task.ID && x.BelongsToUser == user.ID && x.BelongsToFromUser == colocutor.ID) != null) continue;
                    _notifications.Add(new Notifications {
                        Created = DateTime.Now,
                        NotificationType = notifyType,
                        PublicKey = Guid.NewGuid(),
                        Reviewed = false,
                        BelongsToTask = task.ID,
                        BelongsToUser = user.ID,
                        BelongsToFromUser = colocutor.ID
                    });
                    hasChanges = true;
                }
                if (hasChanges) _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<bool> AddNotifications(string notifyType, Guid taskPk, List<Guid> usersPk, Guid fromUserPk) {
            try {
                var hasChanges = false;
                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                var colocutor = _users.GetSingleOrDefault(x => x.PublicKey == fromUserPk);

                foreach (var userPk in usersPk) {
                    var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                    if (task == null || user == null || colocutor == null) continue;
                    if (_notifications.GetFirstOrDefault(x => x.BelongsToTask == task.ID && x.BelongsToUser == user.ID && x.BelongsToFromUser == colocutor.ID) != null) continue;
                    var notyPk = Guid.NewGuid();
                    _notifications.Add(new Notifications {
                        Created = DateTime.Now,
                        NotificationType = notifyType,
                        PublicKey = notyPk,
                        Reviewed = false,
                        BelongsToTask = task.ID,
                        BelongsToUser = user.ID,
                        BelongsToFromUser = colocutor.ID
                    });
                    hasChanges = true;
                }
                if (hasChanges) _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<List<Guid>> GetMyProposals(Guid userPk) {
            try {
                var result = new List<Guid>();
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<List<Guid>>(new Exception("SetNotificationAsViewed: user with PK: " + userPk + " doesnt found."));

                var notifies = _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.NotificationType == NotifycationTypes.ProposalOfTask.ToString()).ToList();
                if (notifies.Count == 0) return new OperationResult<List<Guid>>(new Exception("SetNotificationAsViewed: notifies belongs to user with pk: " + userPk + " doesnt found."));

                foreach (var noty in notifies) {
                    var task = _tasks.GetSingleOrDefault(x => x.ID == noty.BelongsToTask);
                    if (task != null) result.Add(task.PublicKey);
                }
                return new OperationResult<List<Guid>>(result);
            }
            catch (Exception ex) {
                return new OperationResult<List<Guid>>(ex);
            }
        }



        public OperationResult<bool> SetProposalNotificationsAsViewed(Guid userPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: user with PK: " + userPk + " doesnt found."));

                var notifies = _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.NotificationType == NotifycationTypes.ProposalOfTask.ToString()).ToList();
                if (notifies.Count == 0) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: notify with type: [ProposalOfTask] for user " + userPk + " doesnt found."));

                foreach (var notify in notifies) {
                    notify.Reviewed = true;
                }
                _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<bool> SetTaskNotificationAsViewed(Guid userPk, Guid taskPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: user with PK: " + userPk + " doesnt found."));

                var task = _tasks.GetSingleOrDefault(x => x.PublicKey == taskPk);
                if (task == null) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: task with PK: " + taskPk + " doesnt found."));

                var notifies = _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.BelongsToTask == task.ID).ToList();
                foreach (var notify in notifies) {
                    notify.Reviewed = true;
                }
                _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<bool> SetMessageNotificationAsViewed(Guid userPk, Guid collocutorPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: user with PK: " + userPk + " doesnt found."));

                var collocutor = _users.GetSingleOrDefault(x => x.PublicKey == collocutorPk);
                if (collocutor == null) return new OperationResult<bool>(new Exception("SetNotificationAsViewed: user with PK: " + collocutorPk + " doesnt found."));

                var notify = _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.BelongsToFromUser == collocutor.ID).ToList();
                foreach (var noty in notify) {
                    noty.Reviewed = true;
                }
                _notifications.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<List<ApplicationNotifycation>> GetMyNotifications(Guid userId, bool justActual) { 
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                var notifications = justActual ? 
                    _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.Reviewed == false) : 
                    _notifications.GetFiltered(x => x.BelongsToUser == user.ID && x.Reviewed);

                var anotifications = new List<ApplicationNotifycation>();
                foreach (var notification in notifications) {
                    var anotification = (ApplicationNotifycation) notification;

                    var task = _tasks.GetSingleOrDefault(x => x.ID == notification.BelongsToTask);
                    anotification.Task = new { Pk = task.PublicKey, Title = task.Name };

                    var sender = _users.GetSingleOrDefault(x => x.ID == notification.BelongsToFromUser);
                    anotification.FromUser = new { Pk = sender.PublicKey, Name = sender.Name, Logo = sender.UserLogoPath };
                    anotifications.Add(anotification);
                }
                return new OperationResult<List<ApplicationNotifycation>>(anotifications);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationNotifycation>>(ex);
            }
        }

    }
}
