using System;
using System.Collections.Generic;
using System.Linq;
using Avelango.Handlers.Encryption;
using Avelango.Models.Abstractions.Db;
using Avelango.Models.Abstractions.UnitOfWork;
using Avelango.Models.Accessory;
using Avelango.Models.Application;
using Avelango.Models.Enums;
using Avelango.Models.Orm;

namespace Avelango.DbOrm.Implementation
{
    public class ImpUser : IUsers
    {
        private readonly IRepository<Users> _users;
        private readonly IRepository<Tasks> _tasks;
        private readonly IRepository<SmsSending> _sms;
        private readonly IRepository<Portfolios> _portfolios;
        private readonly IRepository<PortfolioJobs> _portfolioJobs;
        private readonly IRepository<PortfolioJobImages> _portfolioJobImages;
        private readonly IRepository<TempUserData> _tempUserData;

        public ImpUser(IRepository<Users> users, IRepository<Portfolios> portfolios, IRepository<Tasks> tasks, IRepository<TempUserData> tempUserData, IRepository<SmsSending> sms, IRepository<PortfolioJobs> portfolioJobs, IRepository<PortfolioJobImages> portfolioJobImages) {
            _users = users;
            _portfolios = portfolios;
            _tasks = tasks;
            _tempUserData = tempUserData;
            _sms = sms;
            _portfolioJobs = portfolioJobs;
            _portfolioJobImages = portfolioJobImages;
        }


        public OperationResult<List<ApplicationUser>> GetFilteredUsers(List<Guid> viewedUsers, List<string> subgroups, double? placeLat, double? placeLng, List<Guid> usersOnline, int countOfUsers) {
            var result = new List<ApplicationUser>();
            IEnumerable<Users> users;
            if (viewedUsers.Any()) {
                if (subgroups.Any()) {
                    if (usersOnline.Any()) {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid()) && subgroups.Contains(x.SubscribeToGroups)
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng
                                                    && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid()) && subgroups.Contains(x.SubscribeToGroups)
                                                    && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng
                                                    && subgroups.Contains(x.SubscribeToGroups)).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())
                                                    && subgroups.Contains(x.SubscribeToGroups)).Take(countOfUsers);
                        }
                    }
                }
                else {
                    if (usersOnline.Any()) {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng
                                                    && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())
                                                    && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng)
                                                    .Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => !viewedUsers.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                    }
                }
            }
            else {
                if (subgroups.Any()) {
                    if (usersOnline.Any()) {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => subgroups.Contains(x.SubscribeToGroups)
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng
                                                    && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => subgroups.Contains(x.SubscribeToGroups) && !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => subgroups.Contains(x.SubscribeToGroups)
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => subgroups.Contains(x.SubscribeToGroups)).Take(countOfUsers);
                        }
                    }
                }
                else {
                    if (usersOnline.Any()) {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => !usersOnline.Contains(x.PublicKey ?? new Guid())
                                                    && placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(countOfUsers).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll(x => !usersOnline.Contains(x.PublicKey ?? new Guid())).Take(countOfUsers);
                        }
                    }
                    else {
                        if (placeLat != null && placeLng != null) {
                            users = _users.GetAll(x => placeLat + 0.3 > x.PlaceLat && placeLat - 0.3 < x.PlaceLat && placeLng + 0.3 > x.PlaceLng && placeLng - 0.3 < x.PlaceLng).Take(countOfUsers);
                        }
                        else {
                            users = _users.GetAll().Take(countOfUsers);
                        }
                    }
                }
            }
            foreach (var user in users) {
                var appUser = (ApplicationUser)user;
                result.Add(appUser);
            }
            return new OperationResult<List<ApplicationUser>>(result);
        }



        public OperationResult<List<ApplicationTask>> GetMyOpenedTasks(Guid myPk) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == myPk);
                IEnumerable<Tasks> tasks = new List<Tasks>();
                if (user != null) {
                    tasks = _tasks.GetFiltered(x => x.Customer == user.ID && x.ProccessStatus == TaskStatus.Open.ToString());
                }
                var atask = tasks.Select(i4 => (ApplicationTask)i4).ToList();
                return new OperationResult<List<ApplicationTask>>(atask);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationTask>>(ex);
            }
        }


        public OperationResult<bool> CheckPhone(string phone) {
            try {
                var exist = _users.GetSingleOrDefault(x => x.Phone1 == phone || x.Phone2 == phone || x.Phone3 == phone);
                return exist == null ? new OperationResult<bool>(false) : new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> CheckSmsSendingEnable(string phone) {
            try {
                var sender = _sms.GetSingleOrDefault(x => x.Phone == phone);
                if (sender == null) return new OperationResult<bool>(true);

                if (sender.Counter <= 1) return new OperationResult<bool>(true);
                if ((sender.ResetTime ?? DateTime.Now).AddDays(1) >= DateTime.Now) return new OperationResult<bool>(false);
                sender.Counter = 0;
                _sms.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> AddSendedSms(string ip, string phone) {
            try {
                var sender = _sms.GetSingleOrDefault(x => x.Phone == phone);
                if (sender == null) {
                    _sms.Add(new SmsSending { Counter = 1, PublicKey = Guid.NewGuid(), ResetTime = DateTime.Now, Ip = ip, Phone = phone });
                }
                else {
                    sender.Counter++;
                    sender.ResetTime = DateTime.Now;
                }
                _sms.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        public OperationResult<int> GetTempPassword() {
            try {
                var password = 0;
                for (var i = 0; i < 99999999; i++) {
                    password = new Random().Next(10000000, 99999999);
                    var hash = Md5.GetHashString(password.ToString());
                    var exist = _users.GetSingleOrDefault(x => x.PasswordHash == hash);
                    if (exist == null) break;
                }
                return new OperationResult<int>(password);
            }
            catch (Exception ex) {
                return new OperationResult<int>(ex);
            }
        }


        public ApplicationUser GetUserInfo(string phone, string password) {
            var hash = Md5.GetHashString(password);
            var user = _users.GetSingleOrDefault(x => (x.Phone1 == phone || x.Phone2 == phone || x.Phone3 == phone) && x.PasswordHash == hash);
            return user == null ? null : (ApplicationUser)user;
        }


        public ApplicationUser GetUserInfo(Guid pk) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == pk);
            return user == null ? null : (ApplicationUser)user;
        }


        public ApplicationUser GetFullUserInfo(Guid userPk) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userPk);
            var fullUserInfo = (ApplicationUser)user;

            var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
            var appPortfolio = (ApplicationPortfolio)portfolio;
            if (portfolio != null) {
                var portfolioJobs = _portfolioJobs.GetFiltered(x => x.BelongsToPortfolio == portfolio.ID).ToList();
                if (portfolioJobs.Any())
                {
                    foreach (var portfolioJob in portfolioJobs)
                    {
                        var portfolioJobImages = _portfolioJobImages.GetFiltered(x => x.BelongsToPortfolioJob == portfolioJob.ID);

                        var applicationPortfolioJobs = (ApplicationPortfolioJobs)portfolioJob;
                        applicationPortfolioJobs.Images = new List<ApplicationPortfolioJobImages>();
                        foreach (var portfolioJobImage in portfolioJobImages)
                        {
                            applicationPortfolioJobs.Images.Add((ApplicationPortfolioJobImages)portfolioJobImage);
                        }
                        appPortfolio.Jobs.Add(applicationPortfolioJobs);
                    }
                    fullUserInfo.Portfolio = appPortfolio;
                }
            }
            fullUserInfo.Comments = new List<ApplicationComments>();
            return fullUserInfo;
        }


        public OperationResult<bool> UserChecked(Guid pk, bool success) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == pk);
                if (success) {
                    user.IsActive = true;
                    user.IsModerChecked = true;
                }
                else {
                    user.IsActive = false;
                    user.IsModerChecked = true;
                }
                _users.UnitOfWork.Commit();
                return new OperationResult<bool>(true);
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<List<ApplicationUser>> GetUsersAwaitedModerators() {
            try {
                var res = new List<ApplicationUser>();
                var users = _users.GetFiltered(x => !x.IsModerChecked);
                foreach (var user in users) {
                    var auser = (ApplicationUser) user;
                    var portfolio = _portfolios.GetSingleOrDefault(x => x.BelongsToUser == user.ID);
                    ApplicationPortfolio aportfolio = null;
                    if (portfolio != null) aportfolio = (ApplicationPortfolio) portfolio;
                    auser.Portfolio = aportfolio;
                    res.Add(auser);
                }
                return new OperationResult<List<ApplicationUser>>(res);
            }
            catch (Exception ex) {
                return new OperationResult<List<ApplicationUser>>(ex);
            }
        }


        public IEnumerable<ApplicationUser> GetUsersInfo() {
            var users = _users.GetAll(x => x.IsAdmin == false && x.IsModerator == false);
            return users.Select(user => (ApplicationUser)user).ToList();
        }


        public IEnumerable<ApplicationUser> GetModeratorsInfo() {
            var users = _users.GetFiltered(x => x.IsModerator == true);
            return users.Select(user => (ApplicationUser)user).ToList();
        }


        public OperationResult<bool> RemoveUser(string id, bool removeJustEnabled) {
            try {
                Guid pk;
                var done = Guid.TryParse(id, out pk);
                if (!done) return new OperationResult<bool>(new Exception("Method RemoveUser. " + id + "have not been parsed."));
                var user = _users.GetSingleOrDefault(x => x.PublicKey == pk);
                if (user == null) return new OperationResult<bool>(new Exception("Method RemoveUser. User with id=" + id + "have not been found."));

                if (removeJustEnabled) {
                    if (!user.IsEnabled) return new OperationResult<bool>();
                }
                _users.Remove(user);
                _users.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
       


        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        public OperationResult<ApplicationUser> Register(ApplicationUser appUser) {
            try {
                var user = _users.GetSingleOrDefault(x => x.Phone1 == appUser.PhoneNumber || x.Phone2 == appUser.PhoneNumber || x.Phone3 == appUser.PhoneNumber);
                if (user != null) return null;
                _users.Add(new Users {
                    PublicKey = Guid.NewGuid(),
                    IsCompany = false,
                    Name = appUser.Name,
                    SurName = string.Empty,
                    Email = string.Empty,
                    PasswordHash = Md5.GetHashString(appUser.PasswordHash),
                    Phone1 = appUser.PhoneNumber,
                    AccountCreated = DateTime.Now,
                    LastLogIn = DateTime.Now,
                    Rating = 0,
                    SubscribeToGroups = "[]",
                    IsEnabled = true,
                    IsModerChecked = true,
                    ReceiveNews = true,
                    Lang = string.Empty,
                    IsAdmin = false,
                    IsModerator = false,
                    IsActive = true
                });
                _users.UnitOfWork.Commit();
                user = _users.GetSingleOrDefault(x => x.Phone1 == appUser.PhoneNumber || x.Phone2 == appUser.PhoneNumber || x.Phone3 == appUser.PhoneNumber);

                if (user == null) throw new Exception("[ImpUsers]/[Register]: User doesn't created");
                return new OperationResult<ApplicationUser>((ApplicationUser)user);
            }
            catch(Exception ex) {
                return new OperationResult<ApplicationUser>(ex);
            }
        }


        /// <summary>
        /// Confirm
        /// </summary>
        /// <param name="userGuid"></param>
        /// <returns></returns>
        public ApplicationUser Confirm(Guid userGuid) {
            var user = _users.GetSingleOrDefault(x => x.PublicKey == userGuid);
            user.IsEnabled = true;
            _users.UnitOfWork.Commit();
            return (ApplicationUser)user;
        }


        /// <summary>
        /// Confirm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public OperationResult<bool> CheckEmailExist(string email) {
            try {
                var user = _users.GetSingleOrDefault(x => x.Email.ToLower().Trim() == email.ToLower().Trim());
                return new OperationResult<bool>(user != null); ;
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> SetImages(Guid userId, string logoPath, string logoPathMax) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                if (user == null) return new OperationResult<bool>(new Exception("Method SetImages. User with id=" + userId + "have not been found."));
                user.UserLogoPath = logoPath;
                user.UserLogoPathMax = logoPathMax;
                user.IsModerChecked = false;
                _users.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> ChangeUserInfo(ApplicationUser apuser) {
            try {
                var user = _users.GetFirstOrDefault(x => x.PublicKey == apuser.Pk);
                if (user == null) return new OperationResult<bool>(new Exception("Method SetImages. User with id=" + apuser.Id + "have not been found."));
                user.Name = apuser.Name ?? string.Empty;
                user.SurName = apuser.SurName ?? string.Empty;
                user.Phone2 = apuser.Phone2 ?? string.Empty;
                user.Phone3 = apuser.Phone3 ?? string.Empty;
                user.Skype = apuser.Skype ?? string.Empty;
                user.Email = apuser.Email ?? string.Empty;
                user.Birthday = apuser.Birthday;
                user.Gender = apuser.Gender;
                user.PlaceName = apuser.PlaceName ?? string.Empty;
                user.PlaceLat = apuser.PlaceLat ?? 0;
                user.PlaceLng = apuser.PlaceLng ?? 0;
                user.IsModerChecked = false;
                _users.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> SetTempUserInfo(Guid userPk, string  userName, string userSurName) {
            try {
                var user = _users.GetFirstOrDefault(x => x.PublicKey == userPk);
                if (user == null) return new OperationResult<bool>(new Exception("[ImpUsers]/[SetTempUserInfo] User with Pk: " + userPk + " doesnt found."));
                _tempUserData.Add(new TempUserData {
                    PublicKey = Guid.NewGuid(),
                    BelongsToUser = user.ID,
                    Name = userName,
                    SurName = userSurName,
                });
                _tempUserData.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<bool> SetSubscribedGroups(Guid userId, string groups, bool emailDelivery, bool smsDelivery, bool pushUpDelivery) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                if (user == null) return new OperationResult<bool>(new Exception("GetSubscribedGroups: user does not found"));
                user.SubscribeToGroups = groups;
                user.EmailDelivery = emailDelivery;
                user.SmsDelivery = smsDelivery;
                user.PushUpDelivery = pushUpDelivery;
                _users.UnitOfWork.Commit();
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        public OperationResult<ApplicationUserSubscriptions> GetSubscribedGroups(Guid userId) {
            try {
                var user = _users.GetSingleOrDefault(x => x.PublicKey == userId);
                return user != null ? new OperationResult<ApplicationUserSubscriptions>(new ApplicationUserSubscriptions(user.SubscribeToGroups, user.EmailDelivery, user.SmsDelivery, user.PushUpDelivery)) : 
                                      new OperationResult<ApplicationUserSubscriptions>(new Exception("GetSubscribedGroups: user does not found"));
            }
            catch (Exception ex) {
                return new OperationResult<ApplicationUserSubscriptions>(ex);
            }
        }
    }
}
