using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Resources;

namespace Avelango.Handlers.Lang
{
    public static class PageLangManager
    {
        // GROUPS
        public static readonly Dictionary<string, object> GroupsRu = GetRs(Resources.Groups.Ru.Groups.ResourceManager);
        public static readonly Dictionary<string, object> GroupsDe = GetRs(Resources.Groups.De.Groups.ResourceManager);
        public static readonly Dictionary<string, object> GroupsEn = GetRs(Resources.Groups.En.Groups.ResourceManager);
        public static readonly Dictionary<string, object> GroupsEs = GetRs(Resources.Groups.Es.Groups.ResourceManager);
        public static readonly Dictionary<string, object> GroupsUa = GetRs(Resources.Groups.Ua.Groups.ResourceManager);
        public static readonly Dictionary<string, object> GroupsFr = GetRs(Resources.Groups.Fr.Groups.ResourceManager);

        // GROUPS
        public static readonly Dictionary<string, object> GroupIcons = GetRs(Resources.Groups.GroupIcons.ResourceManager);

        // NOTIFY
        public static readonly Dictionary<string, object> NotifyRu = GetRs(Resources.Notifications.Ru.NotifyContext.ResourceManager);
        public static readonly Dictionary<string, object> NotifyDe = GetRs(Resources.Notifications.De.NotifyContext.ResourceManager);
        public static readonly Dictionary<string, object> NotifyEn = GetRs(Resources.Notifications.En.NotifyContext.ResourceManager);
        public static readonly Dictionary<string, object> NotifyEs = GetRs(Resources.Notifications.Es.NotifyContext.ResourceManager);
        public static readonly Dictionary<string, object> NotifyUa = GetRs(Resources.Notifications.Ua.NotifyContext.ResourceManager);
        public static readonly Dictionary<string, object> NotifyFr = GetRs(Resources.Notifications.Fr.NotifyContext.ResourceManager);

        // LAYOUT
        public static readonly Dictionary<string, object> LayoutRu = GetRs(Resources.Pages.Ru.Layout.ResourceManager);
        public static readonly Dictionary<string, object> LayoutDe = GetRs(Resources.Pages.De.Layout.ResourceManager);
        public static readonly Dictionary<string, object> LayoutEn = GetRs(Resources.Pages.En.Layout.ResourceManager);
        public static readonly Dictionary<string, object> LayoutEs = GetRs(Resources.Pages.Es.Layout.ResourceManager);
        public static readonly Dictionary<string, object> LayoutUa = GetRs(Resources.Pages.Ua.Layout.ResourceManager);
        public static readonly Dictionary<string, object> LayoutFr = GetRs(Resources.Pages.Fr.Layout.ResourceManager);

        // MODAL CREATE ORDER
        public static readonly Dictionary<string, object> ModalAddOrderRu = GetRs(Resources.Modals.Ru.CreateOrder.ResourceManager);
        public static readonly Dictionary<string, object> ModalAddOrderDe = GetRs(Resources.Modals.De.CreateOrder.ResourceManager);
        public static readonly Dictionary<string, object> ModalAddOrderEn = GetRs(Resources.Modals.En.CreateOrder.ResourceManager);
        public static readonly Dictionary<string, object> ModalAddOrderEs = GetRs(Resources.Modals.Es.CreateOrder.ResourceManager);
        public static readonly Dictionary<string, object> ModalAddOrderUa = GetRs(Resources.Modals.Ua.CreateOrder.ResourceManager);
        public static readonly Dictionary<string, object> ModalAddOrderFr = GetRs(Resources.Modals.Fr.CreateOrder.ResourceManager);

        // CAUSES
        public static readonly Dictionary<string, object> CausesDe = GetRs(Resources.Causes.Ru.Content.ResourceManager);
        public static readonly Dictionary<string, object> CausesRu = GetRs(Resources.Causes.De.Content.ResourceManager);
        public static readonly Dictionary<string, object> CausesEs = GetRs(Resources.Causes.En.Content.ResourceManager);
        public static readonly Dictionary<string, object> CausesUa = GetRs(Resources.Causes.Es.Content.ResourceManager);
        public static readonly Dictionary<string, object> CausesFr = GetRs(Resources.Causes.Ua.Content.ResourceManager);
        public static readonly Dictionary<string, object> CausesEn = GetRs(Resources.Causes.Fr.Content.ResourceManager);

        // MAIN PAGE
        public static readonly Dictionary<string, object> MainRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.Index.ResourceManager), ModalAddOrderRu, LayoutRu, NotifyRu, GroupsRu });
        public static readonly Dictionary<string, object> MainDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.Index.ResourceManager), ModalAddOrderDe, LayoutDe, NotifyDe, GroupsDe });
        public static readonly Dictionary<string, object> MainEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.Index.ResourceManager), ModalAddOrderEn, LayoutEn, NotifyEn, GroupsEn });
        public static readonly Dictionary<string, object> MainEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.Index.ResourceManager), ModalAddOrderEs, LayoutEs, NotifyEs, GroupsEs });
        public static readonly Dictionary<string, object> MainUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.Index.ResourceManager), ModalAddOrderUa, LayoutUa, NotifyUa, GroupsUa });
        public static readonly Dictionary<string, object> MainFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.Index.ResourceManager), ModalAddOrderFr, LayoutFr, NotifyFr, GroupsFr });

        // LOGIN PAGE
        public static readonly Dictionary<string, object> LoginRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.Login.ResourceManager), LayoutRu });
        public static readonly Dictionary<string, object> LoginDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.Login.ResourceManager), LayoutDe });
        public static readonly Dictionary<string, object> LoginEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.Login.ResourceManager), LayoutEn });
        public static readonly Dictionary<string, object> LoginEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.Login.ResourceManager), LayoutEs });
        public static readonly Dictionary<string, object> LoginUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.Login.ResourceManager), LayoutUa });
        public static readonly Dictionary<string, object> LoginFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.Login.ResourceManager), LayoutFr });

        // REGISTRATION PAGE
        public static readonly Dictionary<string, object> RegistrationRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.Registration.ResourceManager), LayoutRu });
        public static readonly Dictionary<string, object> RegistrationDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.Registration.ResourceManager), LayoutDe });
        public static readonly Dictionary<string, object> RegistrationEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.Registration.ResourceManager), LayoutEn });
        public static readonly Dictionary<string, object> RegistrationEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.Registration.ResourceManager), LayoutEs });
        public static readonly Dictionary<string, object> RegistrationUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.Registration.ResourceManager), LayoutUa });
        public static readonly Dictionary<string, object> RegistrationFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.Registration.ResourceManager), LayoutFr });

        // USER PARLOUR
        public static readonly Dictionary<string, object> UserParlourRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.User.ResourceManager), ModalAddOrderRu, LayoutRu, NotifyRu });
        public static readonly Dictionary<string, object> UserParlourDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.User.ResourceManager), ModalAddOrderDe, LayoutDe, NotifyDe });
        public static readonly Dictionary<string, object> UserParlourEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.User.ResourceManager), ModalAddOrderEn, LayoutEn, NotifyEn });
        public static readonly Dictionary<string, object> UserParlourEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.User.ResourceManager), ModalAddOrderEs, LayoutEs, NotifyEs });
        public static readonly Dictionary<string, object> UserParlourUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.User.ResourceManager), ModalAddOrderUa, LayoutUa, NotifyUa });
        public static readonly Dictionary<string, object> UserParlourFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.User.ResourceManager), ModalAddOrderFr, LayoutFr, NotifyFr });

        // EXECUTORS
        public static readonly Dictionary<string, object> ExecutorsRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.User.ResourceManager), GroupsRu });
        public static readonly Dictionary<string, object> ExecutorsDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.User.ResourceManager), GroupsDe });
        public static readonly Dictionary<string, object> ExecutorsEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.User.ResourceManager), GroupsEn });
        public static readonly Dictionary<string, object> ExecutorsEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.User.ResourceManager), GroupsEs });
        public static readonly Dictionary<string, object> ExecutorsUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.User.ResourceManager), GroupsUa });
        public static readonly Dictionary<string, object> ExecutorsFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.User.ResourceManager), GroupsFr });

        // TASKS
        public static readonly Dictionary<string, object> TasksRu = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ru.User.ResourceManager), GroupsRu });
        public static readonly Dictionary<string, object> TasksDe = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.De.User.ResourceManager), GroupsDe });
        public static readonly Dictionary<string, object> TasksEn = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.En.User.ResourceManager), GroupsEn });
        public static readonly Dictionary<string, object> TasksEs = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Es.User.ResourceManager), GroupsEs });
        public static readonly Dictionary<string, object> TasksUa = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Ua.User.ResourceManager), GroupsUa });
        public static readonly Dictionary<string, object> TasksFr = DicsUnion(new List<Dictionary<string, object>> { GetRs(Resources.Pages.Fr.User.ResourceManager), GroupsFr });


        public static Dictionary<string, object> GetPageContent(string page, string lang) {
            switch (page.ToLower()) {
                case "login": {
                    switch (lang.ToLower()) {
                        case "de": { return LoginDe; }
                        case "ru": { return LoginRu; }
                        case "es": { return LoginEs; }
                        case "ua": { return LoginUa; }
                        case "fr": { return LoginFr; }
                        default: { return LoginEn; }
                    }
                }
                case "registration": {
                        switch (lang.ToLower()) {
                            case "de": { return RegistrationDe; }
                            case "ru": { return RegistrationRu; }
                            case "es": { return RegistrationEs; }
                            case "ua": { return RegistrationUa; }
                            case "fr": { return RegistrationFr; }
                            default: { return RegistrationEn; }
                        }
                    }
                case "usercabinet": {
                    switch (lang.ToLower()) {
                            case "de": { return UserParlourDe; }
                            case "ru": { return UserParlourRu; }
                            case "es": { return UserParlourEs; }
                            case "ua": { return UserParlourUa; }
                            case "fr": { return UserParlourFr; }
                            default: { return UserParlourEn; }
                    }
                }
                case "tasks": {
                        switch (lang.ToLower()) {
                            case "de": { return TasksDe; }
                            case "ru": { return TasksRu; }
                            case "es": { return TasksEs; }
                            case "ua": { return TasksUa; }
                            case "fr": { return TasksFr; }
                            default: { return TasksEn; }
                        }
                    }
                case "executors": {
                        switch (lang.ToLower()) {
                            case "de": { return ExecutorsDe; }
                            case "ru": { return ExecutorsRu; }
                            case "es": { return ExecutorsEs; }
                            case "ua": { return ExecutorsUa; }
                            case "fr": { return ExecutorsFr; }
                            default: { return ExecutorsEn; }
                        }
                    }
                default: { // main
                    switch (lang.ToLower()) {
                            case "de": { return MainDe; }
                            case "ru": { return MainRu; }
                            case "es": { return MainEs; }
                            case "ua": { return MainUa; }
                            case "fr": { return MainFr; }
                            default: { return MainEn; }
                        }
                }
            }
        }


        public static Dictionary<string, object> GetRs(ResourceManager res) {
            return res.GetResourceSet(CultureInfo.CurrentUICulture, true, true).Cast<DictionaryEntry>().ToDictionary(r => r.Key.ToString(), r => r.Value);
        }


        public static Dictionary<string, object> GetCauses(string lang) {
            switch (lang.ToLower()) {
                case "de": { return CausesDe; }
                case "ru": { return CausesRu; }
                case "es": { return CausesEs; }
                case "ua": { return CausesUa; }
                case "fr": { return CausesFr; }
                default: { return CausesEn; }
            }
        }


        public static Dictionary<string, object> GetGroupsContent(string lang) {
            switch (lang.ToLower()) {
                case "de": { return GroupsDe; }
                case "ru": { return GroupsRu; }
                case "es": { return GroupsEs; }
                case "ua": { return GroupsUa; }
                case "fr": { return GroupsFr; }
                default: { return GroupsEn; }
            }
        }


        private static Dictionary<string, object> DicsUnion(IEnumerable<Dictionary<string, object>> dics) { //, Dictionary<string, object> groups = null) {
            var result = new Dictionary<string, object>();
            foreach (var item in dics.SelectMany(dic => dic.Where(item => !result.Keys.Contains(item.Key)))) {
                result.Add(item.Key, item.Value);
            }
            //var result = dics.SelectMany(dic => dic).ToDictionary(i4 => i4.Key, i4 => i4.Value);
            return result;
        }
    }
}