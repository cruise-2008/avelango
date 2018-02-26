using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Avelango.Models.Application;
using Avelango.Models.Enums;

namespace Avelango.Handlers.Lang
{
    public static class GroupManager
    {
        public static readonly List<ApplicationGroup> GroupsTreeEn = GetGroupsTree(Langs.LangsEnum.En.ToString());
        public static readonly List<ApplicationGroup> GroupsTreeRu = GetGroupsTree(Langs.LangsEnum.Ru.ToString());
        public static readonly List<ApplicationGroup> GroupsTreeDe = GetGroupsTree(Langs.LangsEnum.De.ToString());
        public static readonly List<ApplicationGroup> GroupsTreeEs = GetGroupsTree(Langs.LangsEnum.Es.ToString());
        public static readonly List<ApplicationGroup> GroupsTreeFr = GetGroupsTree(Langs.LangsEnum.Fr.ToString());
        public static readonly List<ApplicationGroup> GroupsTreeUa = GetGroupsTree(Langs.LangsEnum.Ua.ToString());


        public static List<string> GetGroupNames() {
            var groupNames = PageLangManager.GetGroupsContent(Langs.LangsEnum.En.ToString()).Select(@group => @group.Key).ToList();
            return groupNames;
        }


        public static List<ApplicationGroup> GetGroupTree(string lang) {
            switch (lang.ToLower()) {
                case "de": { return GroupsTreeDe; }
                case "ru": { return GroupsTreeRu; }
                case "es": { return GroupsTreeEs; }
                case "ua": { return GroupsTreeUa; }
                case "fr": { return GroupsTreeFr; }
                default: { return GroupsTreeEn; }
            }
        }


        private static List<ApplicationGroup> GetGroupsTree(string lang) {
            var lgroups = PageLangManager.GetGroupsContent(lang);
            var groupIcons = PageLangManager.GroupIcons;

            var groups = new List<ApplicationGroup>();
            foreach (var lgroup in lgroups) {
                if (lgroup.Key.Length != 1) continue;
                var ico = string.Empty;
                foreach (var groupIcon in groupIcons.Where(groupIcon => lgroup.Key == groupIcon.Key)) {
                    ico = groupIcon.Value.ToString();
                }
                groups.Add(new ApplicationGroup(lgroup.Key, lgroup.Value.ToString(), ico));
            }

            foreach (var i4 in lgroups) {
                if (!Regex.Match(i4.Key, @"\w{1}\d{1,3}").Success) continue;
                var groupName = Regex.Match(i4.Key, @"(\w{1})(\d{1,3})").Groups[1].Value;
                foreach (var group in groups.Where(group => group.Name == groupName)) {
                    group.SubGroups.Add(new SubGroup(i4.Key, i4.Value.ToString()));
                }
            }
            var groupsSorted = groups.Select(group => new ApplicationGroup {
                Name = group.Name,
                Text = group.Text,
                Ico = group.Ico,
                SubGroups = group.SubGroups.OrderBy(x => x.Name).ToList()
            }).ToList();
            return groupsSorted.OrderBy(x => x.Name).ToList();
        }
    }
}