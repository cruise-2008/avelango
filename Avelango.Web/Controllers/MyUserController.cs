using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Avelango.Handlers.Image;
using Avelango.Handlers.Lang;
using Avelango.Hubs.Accessory;
using Avelango.Models.Abstractions.Accessory;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Application;
using Avelango.Models.User;
using Avelango.Web.Models;
using Avelango.Web.Models.Attributes;

namespace Avelango.Web.Controllers
{
    public class MyUserController : Controller
    {
        private readonly ILog _log;
        private readonly IUsers _users;
        private readonly ITasks _tasks;
        private readonly IPortfolios _portfolios;
        private readonly IChatMessages _chatMessages;
        private readonly ICommonNews _news;
        private readonly ICommonAdvertising _advertising;

        public MyUserController(IUsers users, IPortfolios portfolios, ICommonNews news, ICommonAdvertising advertising, IChatMessages chatMessages, ITasks tasks, ILog log) {
            _users = users;
            _portfolios = portfolios;
            _news = news;
            _advertising = advertising;
            _chatMessages = chatMessages;
            _tasks = tasks;
            _log = log;
        }


        // GET: /MyUser/Parlour
        [HttpGet]
        [AccessLevelAnyAutorized]
        public ActionResult Parlour(string coveringData) {
            if(!string.IsNullOrEmpty(coveringData)) ViewBag.CoveringData = coveringData;
            return View();
        }


        // GET: /MyUser/ChangeAvatarModule
        public ActionResult ChangeAvatarModule() {
            return View();
        }



        // POST: /MyUser/GetInitialPageData
        [AccessLevelAnyAutorized]
        public ActionResult GetInitialPageData() {
            try {
                var session = new PrivateSession().Current;
                //var newMessages = _chatMessages.CountNewMessages(session.User.Pk).Data;
                var incompleteTasks = _tasks.CountIncompleteTasks(session.User.Pk).Data;
                var incompleteJobs = _tasks.CountIncompleteJobs(session.User.Pk).Data;

                //var tasks = new List<Task> {
                //    Task.Run(() => newMessages = _chatMessages.CountNewMessages(session.User.Pk).Data),
                //    Task.Run(() => incompleteTasks = _tasks.CountIncompleteTasks(session.User.Pk).Data),
                //    Task.Run(() => incompleteJobs = _tasks.CountIncompleteJobs(session.User.Pk).Data),
                //}.ToArray();
                //Task.WaitAll(tasks);
                return Json(new { IsSuccess = true,
                    //NewMessages = newMessages,
                    IncompleteTasks = incompleteTasks,
                    IncompleteJobs = incompleteJobs
                });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetInitialPageData]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/GetInitialPageData
        [AccessLevelAnyAutorized]
        public ActionResult GetCommonInitialPageData() {
            try {
                var news = new List<ApplicationNews>();
                var advertising = new List<ApplicationAdvertising>();

                var lang = new PrivateSession().Current.CurrentLang;

                var tasks = new List<Task> {
                    Task.Run(() => news = _news.GetAll(lang).Data),
                    Task.Run(() => advertising = _advertising.GetAll().Data)
                }.ToArray();
                Task.WaitAll(tasks);

                return Json(new { IsSuccess = true, InitialPageData = new {
                    News = news,
                    Advertising = advertising,
                }
                });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetCommonInitialPageData]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/MyPageAnothersEyes
        [AccessLevelAnyAutorized]
        public ActionResult MyPageAnothersEyes() {
            return View();
        }


        // GET: /MyUser/AddPortfolioData
        [AccessLevelAnyAutorized]
        public ActionResult AddPortfolioData(string title, string description) {
            try {
                var result = _portfolios.AddPortfolioData(new PrivateSession().Current.User.Pk, title, description);
                return Json(new { IsSuccess = result.IsSuccess });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[AddPortfolioData]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/GetPortfolioData
        [AccessLevelAnyAutorized]
        public ActionResult GetPortfolioData() {
            try {
                var portfolio = _portfolios.GetPortfolio(new PrivateSession().Current.User.Pk);
                if (portfolio == null) return Json(new { Title = "", Description = "", Jobs = new List<object>() });
                return Json( new { IsSuccess = true, Portfolio = portfolio.Data });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetPortfolioData]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/RemovePortfolioJobImage
        [AccessLevelAnyAutorized]
        public ActionResult RemovePortfolioJobImage(Guid jobPk, Guid imagePk) {
            try {
                var imagesRemoved = _portfolios.RemovePortfolioJobImage(new PrivateSession().Current.User.Pk, jobPk, imagePk);
                if (imagesRemoved.IsSuccess)
                {
                    if (string.IsNullOrEmpty(imagesRemoved.Data.Small) || string.IsNullOrEmpty(imagesRemoved.Data.Large)) return Json(new { IsSuccess = false });

                    var imageSmallRemoved = ImgHandler.RemoveImage(Server.MapPath("~") + imagesRemoved.Data.Small);
                    var imageLargeRemoved = ImgHandler.RemoveImage(Server.MapPath("~") + imagesRemoved.Data.Large);
                    if (!imageSmallRemoved || !imageLargeRemoved) return Json(new { IsSuccess = false });

                    return Json(new { IsSuccess = true });
                }
                return Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[RemovePortfolioJobImage]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/RemovePortfolioJob
        [AccessLevelAnyAutorized]
        public ActionResult RemovePortfolioJob(Guid jobPk) {
            try {
                var result = _portfolios.RemovePortfolioJob(new PrivateSession().Current.User.Pk, jobPk);
                return Json(new { IsSuccess = result.IsSuccess });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[RemovePortfolioJob]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/AddPortfolioJobImage
        [AccessLevelAnyAutorized]
        public ActionResult AddPortfolioJobImage(Guid jobPk, IEnumerable<HttpPostedFileBase> files) {
            try {

                var listOfImagePair = SaveFiles(files);
                if (!listOfImagePair.Any()) return Json(new {IsSuccess = false});

                var res = _portfolios.AddPortfolioJobImage(new PrivateSession().Current.User.Pk, jobPk, listOfImagePair);
                if (res.IsSuccess) return Json(new { IsSuccess = true, Images = listOfImagePair });
                return Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[AddPortfolioJobImage]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/ChangePortfolioJobData
        [AccessLevelAnyAutorized]
        public ActionResult ChangePortfolioJobData(Guid jobPk, string title, string description) {
            try {
                var res = _portfolios.ChangePortfolioJobData(new PrivateSession().Current.User.Pk, jobPk, title, description);
                return Json(new { IsSuccess = res, Method = "cpjd" });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[ChangePortfolioJobData]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/PortfolioJobUpLoad
        [AccessLevelAnyAutorized]
        [HttpPost]
        public ActionResult PortfolioJobUpLoad(string title, string description, IEnumerable<HttpPostedFileBase> files) {
            try {
                var listOfImagePair = SaveFiles(files);

                if (listOfImagePair.Any()) {
                    var res = _portfolios.JobUpLoad(new PrivateSession().Current.User.Pk, title, description, listOfImagePair);
                    if (res.IsSuccess) return Json(new { IsSuccess = true, Images = listOfImagePair, Pk = res.Data });
                }
                // If exception
                foreach (var imagePath in listOfImagePair) {
                    ImgHandler.RemoveImage(Server.MapPath("~") + imagePath.Small);
                    ImgHandler.RemoveImage(Server.MapPath("~") + imagePath.Large);
                }
                return Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[PortfolioJobUpLoad]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // GET: /MyUser/GetMyInfo
        [AccessLevelAnyAutorized]
        public ActionResult GetMyInfo() {
            try {
                if (new PrivateSession().Current.User == null) return Json(new { IsSuccess = false });
                var user = _users.GetUserInfo(new PrivateSession().Current.User.Pk);
                return Json(new {
                    Name = user.Name,
                    SurName = user.SurName,
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    PlaceName = user.PlaceName,
                    PlaceLat = user.PlaceLat,
                    PlaceLng = user.PlaceLng,
                    Phone1 = user.Phone1,
                    Phone2 = user.Phone2,
                    Phone3 = user.Phone3,
                    Email = user.Email,
                    Skype = user.Skype,
                    UserLogoPathMax =  string.IsNullOrEmpty(user.UserLogoPathMax) ? Resources.Default.Settings.DefaulLogoPath : user.UserLogoPathMax,
                    IsActive = user.IsActive
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetMyInfo]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/SetMyInfo
        [AccessLevelAnyAutorized]
        public ActionResult SetMyInfo(string name, string surName, string birthday, bool? gender, string placeName, double placeLat, double placeLng, string phone2, string phone3, string email, string skype) {
            try {
                var userPk = new PrivateSession().Current.User.Pk;
                var dataChangeResult = _users.ChangeUserInfo(new ApplicationUser {
                    Pk = userPk,
                    Name = name,
                    SurName = surName,
                    Birthday = birthday,
                    Gender = gender,
                    PlaceName = placeName,
                    PlaceLat = placeLat,
                    PlaceLng = placeLng,
                    Phone2 = phone2,
                    Phone3 = phone3,
                    Email = email,
                    Skype = skype
                });

                if (!string.IsNullOrEmpty(name) || !string.IsNullOrEmpty(surName)) {
                    var addedAsTemp = _users.SetTempUserInfo(userPk, name, surName);
                    if (addedAsTemp.IsSuccess) {
                        HubClient.UserDataChangedForModerator(new JavaScriptSerializer().Serialize( new {
                                UserPk = userPk,
                                Name = name,
                                SurName = surName,
                                Image = string.Empty
                            }));
                    }
                }
                return Json(new { IsSuccess = dataChangeResult.IsSuccess });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[SetMyInfo]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/SetMyInfo
        [AccessLevelAnyAutorized]
        public ActionResult ChangeMyMainPhone(string newPhoneNumber) {
            try {
                return Json(new { IsSuccess = true });
            }
            catch (Exception ex) { 
                _log.AddError("[MyUser]/[ChangeMyMainPhone]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // POST: /MyUser/SetMyLogo
        [AccessLevelAnyAutorized]
        public ActionResult SetMyLogo(string image) {
            try {
                if (image == null) return Json(false);

                // Remove old images
                var user = new PrivateSession().Current.User;
                if (!string.IsNullOrEmpty(user.UserLogoPath)) ImgHandler.RemoveImage(Server.MapPath("~") + user.UserLogoPath);
                if (!string.IsNullOrEmpty(user.UserLogoPathMax)) ImgHandler.RemoveImage(Server.MapPath("~") + user.UserLogoPathMax);

                // Create new images
                var img = ImgHandler.Base64ToImage(image);
                var imgMini = ImgHandler.CreateMiniImage(img, 128, 128);

                // Save new images
                var userLogoPath = @"\Storage\Avatars\" + Guid.NewGuid() + ".png";
                var res1 = ImgHandler.SaveImage(imgMini, Server.MapPath("~") + userLogoPath);

                var userLogoPathMax = @"\Storage\Avatars\" + Guid.NewGuid() + ".png";
                var res2 = ImgHandler.SaveImage(img, Server.MapPath("~") + userLogoPathMax);

                var resSet = false;
                if (res1 && res2) {
                    resSet = _users.SetImages(user.Pk, userLogoPath, userLogoPathMax).IsSuccess;
                }
                user.UserLogoPath = userLogoPath;
                user.UserLogoPathMax = userLogoPathMax;

                if (resSet) return Json(new { IsSuccess = true, UserLogoPath = userLogoPath, UserLogoPathMax = userLogoPathMax });

                ImgHandler.RemoveImage(Server.MapPath("~") + userLogoPath);
                ImgHandler.RemoveImage(Server.MapPath("~") + userLogoPathMax);
                return Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[SetMyLogo]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        private List<ImagePair> SaveFiles(IEnumerable<HttpPostedFileBase> files) {
            try {
                var listOfImagePair = new List<ImagePair>();
                var httpPostedFileBases = files as HttpPostedFileBase[] ?? files.ToArray();
                if (files == null || !httpPostedFileBases.Any()) return listOfImagePair;
                foreach (var file in httpPostedFileBases) {
                    var imgMax = Image.FromStream(file.InputStream);

                    var imgPortfolioMaxPath = @"\Storage\Portfolio\" + Guid.NewGuid() + ".png";
                    var resMax = ImgHandler.SaveImage(imgMax, Server.MapPath("~") + imgPortfolioMaxPath);

                    var imgMini = ImgHandler.CreateMiniImage(imgMax, 128, 128);
                    var imgPortfolioMinPath = @"\Storage\Portfolio\" + Guid.NewGuid() + ".png";
                    var resMin = ImgHandler.SaveImage(imgMini, Server.MapPath("~") + imgPortfolioMinPath);

                    listOfImagePair.Add(new ImagePair { Small = imgPortfolioMinPath, Large = imgPortfolioMaxPath });
                    if (resMax && resMin) continue;
                    break;
                }
                return listOfImagePair;
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[SaveFiles]", ex.Message);
                return new List<ImagePair>();
            }
        }


        // /MyUser/GetMyGroups
        public ActionResult GetMyGroups() {
            try {
                var session = new PrivateSession().Current;
                var groups = PageLangManager.GetGroupsContent(session.CurrentLang.ToString());
                if (session.User == null) return Json(new { IsSuccess = true, Groups = groups });
                var subscribedResult = _users.GetSubscribedGroups(session.User.Pk);
                return subscribedResult.IsSuccess ? Json(new {
                    IsSuccess = true,
                    Groups = groups,
                    Subscribed = subscribedResult.Data.SubscribedGroups,
                    EmailDelivery = subscribedResult.Data.EmailDelivery,
                    SmsDelivery = subscribedResult.Data.SmsDelivery,
                    PushUpDelivery = subscribedResult.Data.PushUpDelivery
                }) : Json(new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetMyGroups]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /MyUser/SetMyGroups
        [AccessLevelAnyAutorized]
        public ActionResult SetMyGroups(string groups, bool emailDelivery, bool smsDelivery, bool pushUpDelivery) {
            try {
                var session = new PrivateSession().Current;
                var subscribedResult = _users.SetSubscribedGroups(session.User.Pk, groups, emailDelivery, smsDelivery, pushUpDelivery);
                return Json(subscribedResult.IsSuccess ? new { IsSuccess = true } : new { IsSuccess = false });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[SetMyGroups]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /MyUser/GetMyCardsTransactions
        [AccessLevelAnyAutorized]
        public ActionResult GetMyCardsTransactions() {
            try {
                return Json(new List<object> {
                    new {
                        Transactions = new List<object> {
                            new { Date = "21-06-2016", Time = "18:15", Data = "Вы получили платеж от Иванова Ивана Ивановича, потери на переводе 1%", Summ = "+100", Ballance = "1280" },
                            new { Date = "22-06-2016", Time = "18:25", Data = "Вы получили платеж от Иванова Ивана Ивановича, потери на переводе 2%", Summ = "+540", Ballance = "1820" },
                            new { Date = "23-06-2016", Time = "18:35", Data = "Вы получили платеж от Иванова Ивана Ивановича, потери на переводе 3%", Summ = "+800", Ballance = "2620" },
                            new { Date = "24-06-2016", Time = "18:45", Data = "Вы получили платеж от Иванова Ивана Ивановича, потери на переводе 4%", Summ = "-1500", Ballance = "1120" },
                            new { Date = "25-06-2016", Time = "18:55", Data = "Вы получили платеж от Иванова Ивана Ивановича, потери на переводе 5%", Summ = "-20", Ballance = "1100" }
                        },
                        Card = new { Number = "9255-1380-3915-3155", Type = "Visa" }
                    }
                });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetMyCardsTransactions]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }


        // /MyUser/GetMyLikedUsers
        [AccessLevelAnyAutorized]
        public ActionResult GetMyLikedUsers() {
            try {
                return Json(new List<object> {
                    new { IsSelected = true, Pk = "6F9619FF-8B86-D011-B42D-00CF4FC964FF", Name = "Bванов Иван Иванович", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png", PortfolioDescription = "Инженер строитель с опытом более одного месяца" },
                    new { IsSelected = true, Pk = "6F9619FF-8B86-D021-B42D-00CF4FC964FF", Name = "Генадий Дольфович Сенрек", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png", PortfolioDescription = "Инженер конструктор со знанием английского" },
                    new { IsSelected = false, Pk = "6F9619FF-8B86-D031-B42D-00CF4FC964FF", Name = "Анна Степановна Гуч", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png", PortfolioDescription = "Фокусник. Интим не прелагать." },
                    new { IsSelected = true, Pk = "6F9619FF-8B86-D041-B42D-00CF4FC964FF", Name = "Константин Иванович Петрушко", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png", PortfolioDescription = "Инженер конструктор со знанием немецкого" },
                    new { IsSelected = false, Pk = "6F9619FF-8B86-D051-B42D-00CF4FC964FF", Name = "Стас Валерьевич Михайлов", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png", PortfolioDescription = "Инженер конструктор со знанием китайского" }
                });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetMyLikedUsers]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }



        // /MyUser/GetMyResponses
        [AccessLevelAnyAutorized]
        public ActionResult GetMyResponses() {
            try {
                return Json(new List<object> {
                    new { DateTime = "22-06-2016 11:15", Pk = "6F9619FF-8B86-D011-B42D-00CF4FC964FF", User= new { Name = "Bванов Иван Иванович", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png"}, JobDescription = "Поговорите с адмиралом Роджерс Бонкс на борту Небесного огня", Message = "Работа выполнена на отлично!", Rating = 5 },
                    new { DateTime = "22-06-2016 12:25", Pk = "6F9619FF-8B86-D021-B42D-00CF4FC964FF", User= new { Name = "Генадий Дольфович Сенрек", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png"}, JobDescription = "Сопроводите Малфуриона к озеру Элуне'ара, чтобы узнать правду об Остаточном хаосе", Message = "Работа выполнена на отлично! Спасибо вам что вы есть!", Rating = 5 },
                    new { DateTime = "22-06-2016 13:35", Pk = "6F9619FF-8B86-D031-B42D-00CF4FC964FF", User= new { Name = "Анна Степановна Гуч", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png"}, JobDescription = "Призыв предводителя рыцарей Кровосмела", Message = "Так себе призван предводитель рыцарей Кровосмела. И на этом спасибо.", Rating = 3 },
                    new { DateTime = "22-06-2016 14:45", Pk = "6F9619FF-8B86-D041-B42D-00CF4FC964FF", User= new { Name = "Константин Иванович Петрушко", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png"}, JobDescription = "С помощью Гароны проникните в Командование Ярости Клинка. Воспользуйтесь ее талантами, чтобы убить стражей и остаться незамеченным ", Message = "Один пират остался жив", Rating = 4 },
                    new { DateTime = "22-06-2016 15:55", Pk = "6F9619FF-8B86-D051-B42D-00CF4FC964FF", User= new { Name = "Стас Валерьевич Михайлов", Logo = @"\Storage\Avatars\bad37900-5c6e-4a1c-b6b6-a03a10890b06.png"}, JobDescription = "Найти доказательства существования гномов Черного Железа", Message = "Доказательства найдены, спасибо!", Rating = 5}
                });
            }
            catch (Exception ex) {
                _log.AddError("[MyUser]/[GetMyResponses]", ex.Message);
                return Json(new { IsSuccess = false });
            }
        }
    }
}