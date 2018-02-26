(function () {
    'use strict';

    angular.module('app.custom')

    .service('signalRSvc', signalRSvc);
    signalRSvc.$inject = ['$rootScope'];

    function signalRSvc($rootScope) {

        var proxy = null;

        var initialize = function (sessionId,groups) {
            //Getting the connection object
            var connection = $.hubConnection();

            //Creating proxy
            proxy = connection.createHubProxy('notificationsHub');

            //Publishing an event when server pushes a greeting message
            proxy.on('NewTaskAddedForModerator', function (message) {//добавлено новое задание или отредакировано (message: taskPk, title   customerName, customerLogoPath)
                $rootScope.$broadcast("newTaskForModer", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "NewTaskForModer" });
            });

            proxy.on('NewTaskAddedForCustomer', function (message) {//добавлено новое задание или отредактировано (message: taskPk)
                $rootScope.$broadcast("newTaskForCustomer", message);
            });

            proxy.on('taskConfirmed', function (message) {//задание отмодерировано и подтверждено (message: taskPk, title)
                $rootScope.$emit("taskConfirmed", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskConfirmed" });
            });

            proxy.on('taskDismissed', function (message) {//задание отмодерировано и отклонено (message: taskPk, title)
                $rootScope.$broadcast("taskDismissed", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('taskBidded', function (message) {//задание биднуто (message: title, workerName, taskPk, workerLogoPath)
                $rootScope.$broadcast("taskBidded", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('taskUnbidded', function (message) {//задание анбиднуто (message: title, workerName, taskPk, workerLogoPath)
                $rootScope.$emit("taskUnbidded", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('customerChosenTheWorkers', function (message) {//заказчик выбрал исполнителей (message: taskPk, title, customerName, customerLogoPath)
                // $rootScope.$emit("customerChosenTheWorkers", message);
                $rootScope.$broadcast("customerChosenTheWorkers", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('workerStartedTask', function (message) {//исполнитель приступил к работе (message: taskPk, title, workerPk, workerName, workerLogoPath)
                $rootScope.$emit("workerStartedTask", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('taskCompletedByCustomer', function (message) {//задание завершено заказчиком (message: taskPk, Title, customerName, customerLogoPath)
                $rootScope.$emit("taskCompletedByCustomer", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('taskCompletedByWorker', function (message) {//задание завершено исполнителем (message: taskPk, Title, workerName, workerLogoPath)
                //$rootScope.$emit("taskCompletedByWorker", message); ----- мб нужно поменять местами с нижним
                $rootScope.$broadcast("taskCompletedByWorker", message);
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "TaskDismissed" });
            });

            proxy.on('taskStateChangedToBusy', function (publicKey) {
                $rootScope.$emit("taskToBusy", publicKey);
            });

            proxy.on('userStateChangedToBusy', function (message) {
                $rootScope.$emit("userToBusy", message);
            });

            proxy.on('workerHasGotProposition', function (message) { // (message: taskPk, customerName, customerLogoPath, title)
                $rootScope.$broadcast("newNotif", { msg: message, notifiType: "ProposalOfTask" });
            });

            proxy.on('workersIsLateToGetTask', function (message) { // (message: taskPk)
                $rootScope.$broadcast("workersIsLateToGetTask", { msg: message, notifiType: "ProposalOfTask" });
            });

            proxy.on('aveRateChanges', function (message) { // message: AveRate
                $rootScope.$broadcast("aveRateChanges", { msg: message });
            });

            proxy.on('rialtoChatMessage', function (message) { // message: AveRate
                $rootScope.$broadcast("rialtoChatMessage", { msg: message });
            });
            


            //Starting connection
            connection.start().done(function () {
                var groupss = JSON.parse(groups);
                proxy.invoke('Connect', groupss, sessionId).done(function () {
                }).fail(function (error) {
                });
            });

            // Disconnection
            $(window).unload(function () {
                proxy.invoke('Disconnect', sessionId).done(function () {
                }).fail(function (error) {
                });
            });
        };
        
        return {
            initialize: initialize
        }
    };

})();

