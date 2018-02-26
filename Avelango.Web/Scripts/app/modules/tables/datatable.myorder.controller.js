/**=========================================================
 * Module: datatable,js
 * Angular Datatable controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('DataTableControllerMyOrder', dataTableControllerMyOrder);

    dataTableControllerMyOrder.$inject = ['$http', '$rootScope', '$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc','starsSvc'];//,'DatatableUsers'
    function dataTableControllerMyOrder($http, $rootScope, $resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, urlSvc, starsSvc) {
        var vm = this;
        activate();
        function activate() {
           // SetNotifiAsViewedSvc.testFu();
            vm.taskBidders = [];
            var url = urlSvc.GetMyOrders;

            $resource(url, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (persons) {
                if (persons.IsSuccess) {
                    persons.Orders.forEach(function (item) {
                        item.Created = item.Created.replace(/\D+/g, '');
                        item.TopicalTo = item.TopicalTo.replace(/\D+/g, '');
                        item.Closed = item.Closed.replace(/\D+/g, '');
                    });
                    vm.heroes = persons.Orders;                 
                } else {
                    vm.heroes = persons;
                }
            });

            $rootScope.$on('indexFormyorder', function (e, message) {
                $scope.numberfororder = message;
            });

            $rootScope.$on('taskConfirmed', function (e, message) {//обновляем таблицу, если какой-то таск отмодерирован
                var msg = JSON.parse(message);
                for (var i = vm.heroes.length-1; i>=0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        vm.heroes[i].Status = 'Open';
                        return;
                    }
                }
            });

            $rootScope.$on('taskDismissed', function (e, message) {//обновляем таблицу, если какой-то таск отмодерирован
                var msg = JSON.parse(message);
                for (var i = vm.heroes.length - 1; i >= 0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        vm.heroes[i].Status = 'Deactivated';
                        return;
                    }
                }
            });

            $rootScope.$on('taskBidded', function (e, message) {//обновляем таблицу, если есть новый бид
                var msg = JSON.parse(message);

                for (var i = vm.heroes.length - 1; i >= 0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        vm.heroes[i].HasBiders++;
                        return;
                    }
                }
            });

            $rootScope.$on('taskUnbidded', function (e, message) {//обновляем таблицу, если бид удален
                var msg = JSON.parse(message);

                for (var i = vm.heroes.length - 1; i >= 0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        vm.heroes[i].HasBiders--;
                        return;
                    }
                }
            });

            $rootScope.$on('workerStartedTask', function (e, message) {//обновляем таблицу, когда кто-то берет в работу задание
                var msg = JSON.parse(message);
                for (var i = vm.heroes.length - 1; i >= 0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        vm.heroes[i].Status = 'InProgress';
                        vm.heroes[i].Worker.Name = msg.workerName;
                        vm.heroes[i].Worker.UserLogoPath = msg.workerPk; // тут в workerPk лежит UserLogoPath 
                        return;
                    }
                }
            });

            $rootScope.$on('taskCompletedByWorker', function (e, message) {//обновляем таблицу, когда задание завершено исполнителем
                var msg = JSON.parse(message);
                for (var i = vm.heroes.length - 1; i >= 0; i--) {
                    if (vm.heroes[i].PublicKey == msg.taskPk) {
                        if (vm.heroes[i].Status == 'InProgress')
                            vm.heroes[i].Status = 'AwaitingCustomerDecision';
                        else if (vm.heroes[i].Status == 'AwaitingWorkerDecision2')
                            vm.heroes[i].Status = 'Closed';
                        return;
                    }
                }
            });

            $rootScope.$on('newTaskForCustomer', function (e, taskPk) {
                $http.post(urlSvc.GetTask, JSON.stringify({ pk: taskPk })).then(function (response) {
                    if (response.data.IsSuccess) {
                        var newTask = response.data.Job[0]; 
                        newTask.Created = newTask.Created.replace(/\D+/g, ''); 
                        newTask.TopicalTo = newTask.TopicalTo.replace(/\D+/g, '');
                        newTask.Closed = newTask.Closed.replace(/\D+/g, '');
                        var temp;
                        for (var i = vm.heroes.length - 1; i >= 0; i--) {
                            if (vm.heroes[i].PublicKey == taskPk) {
                                temp = i;
                            }
                        }
                        if (temp >= 0)
                            vm.heroes[temp] = newTask;
                        else
                            vm.heroes.push(newTask);
                    }
                });
            });

            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1),
                DTColumnDefBuilder.newColumnDef(2),
                DTColumnDefBuilder.newColumnDef(3),
                DTColumnDefBuilder.newColumnDef(4),
                DTColumnDefBuilder.newColumnDef(5),
                DTColumnDefBuilder.newColumnDef(6),
                DTColumnDefBuilder.newColumnDef(7).notSortable()
            ];

            vm.closeOrder = closeOrder;
            vm.removeOrder = removeOrder;
            vm.inDisputOrder = inDisputOrder;
            vm.reOpenOrder = reOpenOrder;
            vm.getBidders = getBidders; 
            vm.appointExecutive = appointExecutive; 
            vm.completeTask = completeTask;

            function getBidders(index, orderPk) {//получаем список биддеров для задания
                vm.currentOrder = orderPk;
                vm.currentOrderId = index;
                    $resource(urlSvc.GetTaskBiders, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                        if (response.IsSuccess) {
                            {
                                response.TaskBiders.forEach(function(item) {
                                    item.Bid.Created = item.Bid.Created.replace(/\D+/g, '');
                                    var r = item.Worker.Rating;
                                    item.Worker.Rating = starsSvc.ratingToMass(r);
                                });
                                vm.taskBidders = response.TaskBiders;
                            }
                        } else {
                            sweetAlert.swal('Cancelled', 'Some Error', 'error');
                        }
                        vm.chekedBidders = [];//тут создается массив в котором будут храниться чекнутые исполнители
                    });
            }

            function appointExecutive() {
                var listExecutors = [];
                for (var i = 0; i < vm.chekedBidders.length; i++) {
                    if (vm.chekedBidders[i])
                        listExecutors.push( vm.taskBidders[i].Worker.PublicKey);
                }
                if (vm.chekedBidders.length > 0) {
                    $resource(urlSvc.SetTaskPreWorkers, { taskPk: vm.currentOrder, executorsPks: listExecutors }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                        if (response.IsSuccess) {
                            sweetAlert.swal('Good job!', 'You have chosen executors!', 'success');
                            vm.heroes[vm.currentOrderId].Status = 'AwaitingWorkerDecision1';
                        }
                    });
                }
            }

            function removeOrder(index, orderPk) {
                sweetAlert.swal({
                    title: 'Are you sure?',
                    text: 'Your will not be able to recover this user!',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel',
                    closeOnConfirm: true,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        $resource(urlSvc.MyOrderRemove, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            console.log(response);
                            if (response.IsSuccess) {
                                sweetAlert.swal('Deleted!', 'Your order has been deleted.', 'success');
                                vm.heroes.splice(index, 1);
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your order is safe :)', 'error');
                    }
                });
            }

            function closeOrder(index, orderPk) {
                $resource(urlSvc.MyOrderClose, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is closed', 'success');
                        vm.heroes[index].Status = 'Closed';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }

            function reOpenOrder(index, orderPk) {
                $resource(urlSvc.MyOrderReopen, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is open', 'success');
                        vm.heroes[index].Status = 'Open';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });              
            }

            function inDisputOrder(index, orderPk) {
                $resource(urlSvc.MyOrderToDisput, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is InDispute', 'success');
                        vm.heroes[index].Status = 'InDispute';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }

            function completeTask(index, orderPk) {

                vm.modalInstance = $uibModal.open({
                    templateUrl: '/commentForExecuterModal.html',
                    controller: ModalCtrl,
                    resolve: {
                        index: function () { return index; },
                        orderPk: function () { return orderPk; }
                    }
                });
            }

            ModalCtrl.$inject = ['$scope', '$uibModalInstance', '$rootScope', 'index', 'orderPk', 'sweetAlert', 'urlSvc'];

            function ModalCtrl($scope, $uibModalInstance, $rootScope, index, orderPk, sweetAlert, urlSvc) {

                $scope.stars = ['fa-star', 'fa-star', 'fa-star', 'fa-star', 'fa-star'];

                $scope.rate = 5;
                $scope.setMark = function (mark) {
                    if (mark + 1 === $scope.rate)
                        $scope.rate = 0;
                    else
                        $scope.rate = mark + 1;
                }

                $scope.outFocusFromStar = function (index) {
                    $scope.stars = ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                    for (var i = 0; i <= $scope.rate - 1; i++)
                        $scope.stars[i] = 'fa-star';
                }

                $scope.onFocusToStar = function(index) {
                    $scope.stars = ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                    for (var i = 0; i <=index; i++)
                        $scope.stars[i] = 'fa-star';
                }

                $scope.mention = "";

                $scope.ok = function() {
                    $resource(urlSvc.MyOrderClose, { taskPk: orderPk, rate: $scope.rate, mention: $scope.mention }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                        $uibModalInstance.dismiss('cancel');
                        if (response.IsSuccess) {
                            if (response.Status === 'Closed') {
                                sweetAlert.swal('Approuved!', 'Your job has closed', 'success');
                                vm.heroes[index].Status = 'Closed';
                            } else if (response.Status === 'AwaitingWorkerDecision2') {
                                sweetAlert.swal('Thank you!', 'Expect to worker decision!', 'success');
                                vm.heroes[index].Status = 'AwaitingWorkerDecision2';
                            }
                        } else {
                            sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                        }
                    });
                }

                $scope.cancel = function() {
                    $uibModalInstance.dismiss('cancel');
                }

                $scope.generatePositiveReview = function () { $scope.mention = "Работник выполнил большой объем работы и уложился в сроки, продемонстрировал способность, принимать правильное решение и смотреть в корень вопроса."; }
                $scope.generateNegativeReview = function () { $scope.mention = "Работник является самым отявленным негодяеи из всех негодяйских негодяев. Сроки сорваны. Работа выполнена ужасно!"; }
            }

            vm.setNotifiAsViewed = function(pk) {
                $resource(urlSvc.SetTaskNotificationAsViewed, { taskPk: pk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {});
            }

        }
    }
})();
