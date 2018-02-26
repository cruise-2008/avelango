using System;
using System.Collections.Generic;
using Avelango.Models.Accessory;
using Avelango.Models.Application;

namespace Avelango.Models.Abstractions.Db
{
    public interface IUsers
    {
        ApplicationUser GetUserInfo(string phone, string password);
        ApplicationUser GetUserInfo(Guid id);
        ApplicationUser GetFullUserInfo(Guid userPk);

        IEnumerable<ApplicationUser> GetUsersInfo();
        IEnumerable<ApplicationUser> GetModeratorsInfo();

        OperationResult<bool> UserChecked(Guid user, bool success);
        OperationResult<int> GetTempPassword();
        OperationResult<bool> AddSendedSms(string ip, string phone);

        OperationResult<bool> CheckSmsSendingEnable(string ip);

        OperationResult<List<ApplicationUser>> GetUsersAwaitedModerators();
        OperationResult<List<ApplicationUser>> GetFilteredUsers(List<Guid> viewedUsers, List<string> subgroups, double? placeLat, double? placeLng, List<Guid> usersOnline, int countOfUsers);

        OperationResult<List<ApplicationTask>> GetMyOpenedTasks(Guid myPk);


        OperationResult<bool> CheckPhone(string phone);
        OperationResult<ApplicationUser> Register(ApplicationUser user);
        ApplicationUser Confirm(Guid user);

        OperationResult<bool> RemoveUser(string id, bool removeJustEnabled);
        OperationResult<bool> CheckEmailExist(string email);
        OperationResult<bool> SetImages(Guid userId, string logoPath, string logoPathMax);
        OperationResult<bool> ChangeUserInfo(ApplicationUser email);
        OperationResult<bool> SetTempUserInfo(Guid userPk, string userName, string userSurName);

        OperationResult<ApplicationUserSubscriptions> GetSubscribedGroups(Guid userId);
        OperationResult<bool> SetSubscribedGroups(Guid userId, string groups, bool emailDelivery, bool smsDelivery, bool pushUpDelivery);

    }
}
