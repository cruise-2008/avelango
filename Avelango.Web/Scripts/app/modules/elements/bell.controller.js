
(function () {
    'use strict';
    angular
        .module('app.elements')
        .controller('BellController', BellController);

    BellController.$inject = ['$resource', '$scope', '$rootScope', '$http', 'urlSvc']; //,'DatatableUsers'

    function BellController($resource, $scope, $rootScope, $http, urlSvc) {

        var vm = this;

        activate();

        function activate() {
            var getNotifi = function(justactual) {
                $resource('/Notifications/GetMyNotifications', { justActual: justactual }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        vm.actualNotifications = response.Notifications;
                        vm.countOfActualNotifications = response.Notifications.length;
                        vm.actualNotificationsHtml = [];
                        response.Notifications.forEach(function (item, i, arr) {
                            item.Created = item.Created.replace(/\D+/g, '');
                            var date = new Date(item.Created * 1);
                            var strDate = date.getFullYear() + "-" + date.getMonth() + "-" + date.getDate();
                            var strTime = date.toTimeString().match(/\d{2}\:\d{2}/g);
                            switch (item.NotificationType) {
                                case "Massage":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Новое сообщение!<br>' + strDate + ' ' + strTime + '</strong><br><span>' + item.FromUser.Name + '(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "ProposalOfTask":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Вам предложили задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "NewTaskForModer":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Поступило новое задание на модерацию!<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskDismissed":
                                    vm.actualNotificationsHtml.push(
                                        '<div><strong style="margin-left: 5%;">К сожалению, ваше задание не рпопшло модерацию<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskConfirmed":
                                    vm.actualNotificationsHtml.push(
                                        '<div><strong style="margin-left: 5%;">Поздравляем! Ваше задание отмодерировано!<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskBidded":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">' + item.FromUser.Name + '&nbsp;откликнулся на ваше задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskUnbidded":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">' + item.FromUser.Name + '&nbsp;передумал выполнять ваше задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "CustomerChosenTheWorkers":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Вас назначили исполнитьелем для задания<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "WorkerStartedTask":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">' + item.FromUser.Name + '&nbsp;приступил выполнять задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskCompletedByCustomer":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Заказчик закрыл задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                case "TaskCompletedByWorker":
                                    vm.actualNotificationsHtml.push(
                                        '<div><div class="col col-xs-3"><img ng-src="' + item.FromUser.Logo + '"/></div><strong style="margin-left: 5%;">Исполнитель завершил задание<br>' + strDate + ' ' + strTime + '</strong><br><span>(' + item.Task.Title + ')</span><\hr></div>');
                                    break
                                default:
                            }
                        });
                    }
                });
            };

            getNotifi(true);

            $rootScope.$on('newNotif', function (e, obj) {//добавляем нотификейшн
                 getNotifi(true);
            });

            vm.redirect = function (type, index, notificationType) {
                switch (notificationType) {
                    case "Massage":
                        window.location.href = "/MyUser/Parlour";
                    case "ProposalOfTask":
                        $resource(urlSvc.SetProposalNotificationsAsViewed, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) { window.location.href = "/Task/Tasks?prop=1"; });
                    case "NewTaskForModer":
                        window.location.href = "/Modderator/Parlour";
                    case "TaskDismissed":
                        window.location.href = "/MyUser/Parlour";
                    case "TaskConfirmed":
                        window.location.href = "/MyUser/Parlour";
                    case "TaskBidded":
                        window.location.href = "/MyUser/Parlour";
                    case "TaskUnbidded":
                        window.location.href = "/MyUser/Parlour";
                    case "CustomerChosenTheWorkers":
                        window.location.href = "/MyUser/Parlour";
                    case "WorkerStartedTask":
                        window.location.href = "/MyUser/Parlour";
                    case "TaskCompletedByCustomer":
                        window.location.href = "/MyUser/Parlour";
                    case "TaskCompletedByWorker":
                        window.location.href = "/MyUser/Parlour";
                }
            }

        }
    }
})();
