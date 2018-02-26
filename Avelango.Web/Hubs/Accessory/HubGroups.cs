using System.Collections.Generic;

namespace Avelango.Hubs.Accessory
{
    public class UserHubGroups
    {
        public List<string> Groups { get; set; }

        public UserHubGroups(string groups) {
            if (Groups == null) Groups = new List<string>();
            Groups.AddRange(groups.Split(','));
        }
    }
}
