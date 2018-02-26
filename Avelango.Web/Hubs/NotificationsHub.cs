using System.Collections.Generic;
using System.Linq;
using Avelango.Hubs.Accessory;
using Microsoft.AspNet.SignalR;

namespace Avelango.Hubs
{
    public class NotificationsHub : Hub {

        /// <summary>
        /// Connect to hub
        /// </summary>
        /// <param name="userGuid"></param>
        /// <param name="userHubGroups"></param>
        public void Connect(List<string> userHubGroups, string userGuid) {
            if (HubClient.Users.Count(x => x.Value.Contains(userGuid)) != 0) RunDisconnect(userGuid);
            HubClient.Users.Add(Context.ConnectionId, userGuid);
            foreach (var group in userHubGroups) {
                Groups.Add(Context.ConnectionId, @group);
            }
        }

        /// <summary>
        /// Disconnect from hub
        /// </summary>
        public void Disconnect(string userGuid) {
            RunDisconnect(userGuid);
        }


        private void RunDisconnect(string userGuid) {
            foreach (var group in HubClient.UserGroups) {
                var contextConnectionId = HubClient.Users.SingleOrDefault(x => x.Value == userGuid).Key;
                if (contextConnectionId != null) Groups.Remove(contextConnectionId, group);
            }
            var item = HubClient.Users.SingleOrDefault(kvp => kvp.Value == userGuid);
            if (item.Key == null) return; {
                HubClient.Users.Remove(item.Key);
                foreach (var i4 in HubClient.TasksInModeration.Where(kvp => kvp.Value == userGuid).ToList()) {
                    HubClient.TasksInModeration.Remove(i4.Key);
                }
                foreach (var i4 in HubClient.UsersInModeration.Where(kvp => kvp.Value == userGuid).ToList()) {
                    HubClient.UsersInModeration.Remove(i4.Key);
                }
            }
        }
    }
}