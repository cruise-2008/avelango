using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Avelango.Models.Accessory;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client;

namespace Avelango.Hubs.Accessory
{
    public static class HubClient {

        public static readonly Dictionary<string, string> Users = new Dictionary<string, string>(); // Context.ConnectionId, userPk
        public static readonly List<string> UserGroups = Handlers.Lang.GroupManager.GetGroupNames();

        public static readonly Dictionary<string, string> TasksInModeration = new Dictionary<string, string>(); // taskPk, moderPk
        public static readonly Dictionary<string, string> UsersInModeration = new Dictionary<string, string>(); // userPk, moderPk


        private static readonly IHubContext HubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationsHub>();

        /// <summary>
        /// Connect to Hub
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userGuid"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static OperationResult<bool> Connect(HttpRequestBase request, string userGuid, List<string> groups) {
            var result = new OperationResult<bool>();
            if (request.Url != null && !string.IsNullOrEmpty(request.ApplicationPath)) {
                var connection = new HubConnection(request.Url.Scheme + "://" + request.Url.Authority + request.ApplicationPath.TrimEnd('/') + "/");
                var proxy = connection.CreateHubProxy(typeof(NotificationsHub).Name);
                connection.Start().ContinueWith(task => {
                    if (task.IsFaulted) {
                        result.Exception = task.Exception;
                        result.IsSuccess = false;
                    }
                    else {
                        var data = new List<object> { groups, userGuid }.ToArray();
                        proxy.Invoke("Connect", data);
                    }
                });
            }
            return result;
        }


        /// <summary>
        /// New Task Added (MODERATOR)
        /// </summary>
        /// <param name="taskPk"></param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> NewTaskAddedForModerator(string taskPk) {
            try {
                HubContext.Clients.Group("mconnect").newTaskAddedForModerator(taskPk);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// New Task Added (MODERATOR)
        /// </summary>
        /// <param name="userData"></param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> UserDataChangedForModerator(string userData) {
            try {
                HubContext.Clients.Group("mconnect").userDataChangedForModerator(userData);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }
  

        /// <summary>
        /// New Task Added (MODERATOR)
        /// </summary>
        /// <param name="userPk">Json object</param>
        /// <param name="taskPk"></param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> NewTaskAddedForCustomer(string userPk, string taskPk) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == userPk.ToString());
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("TaskConfirmed: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).newTaskAddedForCustomer(taskPk);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// New Task Added (USER)
        /// </summary>
        /// <param name="group">String group name</param>
        /// <param name="taskData">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> NewTaskConfirmed(string group, string taskData) {
            try {
                HubContext.Clients.Group(group).newTaskConfirmed(taskData);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// New Task Added (USER)
        /// </summary>
        /// <param name="userPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskConfirmed(string userPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == userPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("TaskConfirmed: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskConfirmed(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// New Task Added (USER)
        /// </summary>
        /// <param name="userPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskDismissed(string userPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == userPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("TaskConfirmed: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskDismissed(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Task Bidded (CUSTOMER)
        /// </summary>
        /// <param name="customerPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskBidded(string customerPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == customerPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("TaskBidded: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskBidded(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Task UnBidded (CUSTOMER)
        /// </summary>
        /// <param name="customerPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskUnbidded(string customerPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == customerPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("TaskUnbidded: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskUnbidded(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Customer Chose Workers (USERS)
        /// </summary>
        /// <param name="usersPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> CustomerChoseWorkers(List<string> usersPk, string data) {
            try {
                foreach (var contextConnectionId in usersPk.Select(userPk => Users.SingleOrDefault(x => x.Value == userPk)).
                    Where(contextConnectionId => contextConnectionId.Key != null)) {
                    HubContext.Clients.Client(contextConnectionId.Key).customerChosenTheWorkers(data);
                }
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Customer Chose Workers (CUSTOMER)
        /// </summary>
        /// <param name="customerPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> WorkerStartedTask(string customerPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == customerPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("WorkerStartedTask: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).workerStartedTask(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Task Completed By Customer (WORKER)
        /// </summary>
        /// <param name="workerPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskCompletedByCustomer(string workerPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == workerPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("WorkerStartedTask: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskCompletedByCustomer(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex)
            {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Task Completed By Worker (CUSTOMER)
        /// </summary>
        /// <param name="customerPk">String group name</param>
        /// <param name="data">Json object</param>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> TaskCompletedByWorker(string customerPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == customerPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("WorkerStartedTask: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).taskCompletedByWorker(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// New Task Added
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> SendChatMessage(string userPk, string message, string attachMin, string attachMax) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == userPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("SendChatMessage: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).newChatMessage(message, attachMin, attachMax);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> MessageToModeratorsTaskStateChanged(string taskPk) {
            try {
                HubContext.Clients.Group("mconnect").taskStateChangedToBusy(taskPk);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> MessageToModeratorsUserStateChanged(string userPk) {
            try {
                HubContext.Clients.Group("mconnect").userStateChangedToBusy(userPk);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> WorkerHasGotProposition(string userPk, string data) {
            try {
                var contextConnectionId = Users.SingleOrDefault(x => x.Value == userPk);
                if (contextConnectionId.Key == null) return new OperationResult<bool>(new Exception("SendChatMessage: User does not found in [Users] group"));
                HubContext.Clients.Client(contextConnectionId.Key).workerHasGotProposition(data);
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> WorkersIsLateToGetTask(List<string> usersPk, string data) {
            try {
                foreach (var contextConnectionId in usersPk.Select(userPk => Users.SingleOrDefault(x => x.Value == userPk)).Where(contextConnectionId => contextConnectionId.Key != null)) {
                    HubContext.Clients.Client(contextConnectionId.Key).workersIsLateToGetTask(data);
                }
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> AveRateChanges(string data) {
            try {
                foreach (var user in Users) {
                    HubContext.Clients.Client(user.Key).aveRateChanges(data);
                }
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }


        /// <summary>
        /// Message To Moderators about Task State Changed
        /// </summary>
        /// <returns>OperationResult</returns>
        public static OperationResult<bool> RialtoChatMessage(string data) {
            try {
                foreach (var user in Users) {
                    HubContext.Clients.Client(user.Key).rialtoChatMessage(data);
                }
                return new OperationResult<bool>();
            }
            catch (Exception ex) {
                return new OperationResult<bool>(ex);
            }
        }



        
    }
}