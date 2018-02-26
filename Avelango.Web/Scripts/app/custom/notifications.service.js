(function() {
    'use strict';

    angular.module('app.custom')
        .service('urlSvc', urlSvc);
    urlSvc.$inject = [];

    function urlSvc($resource) {

        var getMyGroups = '/MyUser/GetMyGroups';
        var GetFilteredTasks = '/Task/GetFilteredTasks';
        var GetTask = '/Task/GetTask';
        var GetFilteredUsers = '/Users/GetFilteredUsers';
        var GetMyOpenedTasks = '/Users/GetMyOpenedTasks';
        var SetOffers = '/Task/SetOffers';
        var AddTask = '/Task/AddTask';
        var MyOrderEdit = '/Task/MyOrderEdit';
        var BidTask = '/Task/BidTask';
        var Login = '/Account/Login';
        var PasswordRecovery = '/Account/PasswordRecovery';
        var Register = '/Account/Register';
        var GetSiteActions = 'Modderator/GetSiteActions';
        var TryToGetTaskOnModeration = '/Hub/TryToGetTaskOnModeration';
        var SetTaskNotificationAsViewed = '/Notifications/SetTaskNotificationAsViewed';
        var GetMyInfo = '/MyUser/GetMyInfo';
        var SetMyInfo = '/MyUser/SetMyInfo';
        var AddPortfolioData = '/MyUser/AddPortfolioData';
        var ChangePortfolioJobData = '/MyUser/ChangePortfolioJobData';
        var GetPortfolioData = '/MyUser/GetPortfolioData';
        var RemovePortfolioJob = '/MyUser/RemovePortfolioJob';
        var RemovePortfolioJobImage = '/MyUser/RemovePortfolioJobImage';
        var GetInitialPageData = '/MyUser/GetInitialPageData';
        var GetCommonInitialPageData = '/MyUser/GetCommonInitialPageData';
        var RemoveUser = '/Addmin/RemoveUser';
        var SetMyGroups = '/MyUser/SetMyGroups';
        var GetMyCardsTransactions = '/MyUser/GetMyCardsTransactions';
        var GetMyOrders = '/Task/GetMyOrders';
        var GetTaskBiders = '/Task/GetTaskBiders';
        var SetTaskPreWorkers = '/Task/SetTaskPreWorkers';
        var MyOrderRemove = '/Task/MyOrderRemove';
        var MyOrderClose = '/Task/MyOrderClose';
        var MyOrderReopen = '/Task/MyOrderReopen';
        var MyOrderToDisput = '/Task/MyOrderToDisput';
        var GetUsersInfo = '/Addmin/GetUsersInfo';
        var GetMyLikedUsers = '/MyUser/GetMyLikedUsers';
        var GetMyChats = '/Chat/GetMyChats';
        var GetChatMessages = '/Chat/GetChatMessages';
        var SetMessageNotificationAsViewed = '/Notifications/SetMessageNotificationAsViewed';
        var SendMessage = '/Chat/SendMessage';
        var GetMyResponses = '/MyUser/GetMyResponses';
        var GetMyJobs = '/Task/GetMyJobs';
        var TaskStartWorking = '/Task/TaskStartWorking';
        var TaskChecked = 'Modderator/TaskChecked';
        var TryToGetUserOnModeration = '/Hub/TryToGetUserOnModeration';
        var UserChecked = 'Modderator/UserChecked';
        var SetCallBack = '/Base/SetCallBack';
        var GetState = '/Base/GetState';
        var SetLang = '/I18N/SetLang';
        var getGroupsTranslate = '/I18N/GetGroupsContent';
        var SetProposalNotificationsAsViewed = '/Notifications/SetProposalNotificationsAsViewed';

        return {
            getGroupsTranslate: getGroupsTranslate,
            getMyGroups: getMyGroups,
            GetFilteredTasks: GetFilteredTasks,
            GetTask: GetTask,
            GetFilteredUsers: GetFilteredUsers,
            GetMyOpenedTasks: GetMyOpenedTasks,
            SetOffers: SetOffers,
            AddTask: AddTask,
            MyOrderEdit: MyOrderEdit,
            BidTask: BidTask,
            Login: Login,
            PasswordRecovery: PasswordRecovery,
            Register: Register,
            GetSiteActions: GetSiteActions,
            TryToGetTaskOnModeration: TryToGetTaskOnModeration,
            SetTaskNotificationAsViewed: SetTaskNotificationAsViewed,
            GetMyInfo: GetMyInfo,
            SetMyInfo: SetMyInfo,
            AddPortfolioData: AddPortfolioData,
            ChangePortfolioJobData: ChangePortfolioJobData,
            GetPortfolioData: GetPortfolioData,
            RemovePortfolioJob: RemovePortfolioJob,
            RemovePortfolioJobImage: RemovePortfolioJobImage,
            GetInitialPageData: GetInitialPageData,
            GetCommonInitialPageData: GetCommonInitialPageData,
            RemoveUser: RemoveUser,
            SetMyGroups: SetMyGroups,
            GetMyCardsTransactions: GetMyCardsTransactions,
            GetMyOrders: GetMyOrders,
            GetTaskBiders: GetTaskBiders,
            SetTaskPreWorkers: SetTaskPreWorkers,
            MyOrderRemove: MyOrderRemove,
            MyOrderClose: MyOrderClose,
            MyOrderReopen: MyOrderReopen,
            MyOrderToDisput: MyOrderToDisput,
            GetUsersInfo: GetUsersInfo,
            GetMyLikedUsers: GetMyLikedUsers,
            GetMyChats : GetMyChats,
            GetChatMessages: GetChatMessages,
            SetMessageNotificationAsViewed: SetMessageNotificationAsViewed,
            SendMessage: SendMessage,
            GetMyResponses: GetMyResponses,
            GetMyJobs: GetMyJobs,
            TaskStartWorking: TaskStartWorking,
            TaskChecked: TaskChecked,
            TryToGetUserOnModeration: TryToGetUserOnModeration,
            UserChecked: UserChecked,
            SetCallBack: SetCallBack,
            GetState: GetState,
            SetLang: SetLang,
            SetProposalNotificationsAsViewed: SetProposalNotificationsAsViewed,
    }
    }
})();