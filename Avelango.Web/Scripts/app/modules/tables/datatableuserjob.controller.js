(function () {
    'use strict';
    angular
        .module('app.tables')
        .controller('DataTableControllerUserJob', dataTableControllerUserJob);

    dataTableControllerUserJob.$inject = ['$rootScope','$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', 'urlSvc'];//,'DatatableUsers'
    function dataTableControllerUserJob($rootScope, $resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope,urlSvc) {
        var vm = this;

        activate();

        function activate() {
            // Ajax
            $resource(urlSvc.GetMyJobs, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (persons) {
                if (persons.IsSuccess) {
                    persons.Jobs.forEach(function (item) {
                        item.Created = item.Created.replace(/\D+/g, '');
                        if (item.DateEnd) item.DateEnd = item.Closed.replace(/\D+/g, '');
                        if (!item.ApprovedToMe && (item.Status === 'inProgress' || item.Status === 'AwaitingWorkerDecision2' || item.Status === 'AwaitingWorkerDecision1' || item.Status === 'AwaitingCustomerDecision' || item.Status === 'Closed')) {
                            item.Status = 'Denied';
                        }
                    });
                    vm.heroess = persons.Jobs;
                }
            });

            $rootScope.$on('customerChosenTheWorkers', function (e, message) {
                var msg = JSON.parse(message);
                for (var i = vm.heroess.length - 1; i >= 0; i--) {
                    if (vm.heroess[i].TaskPublicKey == msg.taskPk) {
                        vm.heroess[i].Status = 'AwaitingWorkerDecision1';
                        vm.heroess[i].ApprovedToMe = true;
                        return;
                    }
                }
            });

            $rootScope.$on('taskCompletedByCustomer', function (e, message) {//обновляем таблицу, когда задание завершено заказчиком
                var msg = JSON.parse(message);
                for (var i = vm.heroess.length - 1; i >= 0; i--) {
                    if (vm.heroess[i].TaskPublicKey == msg.taskPk) {
                        if (vm.heroess[i].Status == 'InProgress')
                            vm.heroess[i].Status = 'AwaitingWorkerDecision2';
                        else if (vm.heroess[i].Status == 'AwaitingCustomerDecision')
                            vm.heroess[i].Status = 'Closed';
                        return;
                    }
                }
            });

            $rootScope.$on('workersIsLateToGetTask', function (e, message) {
                var msg = JSON.parse(message);
                for (var i = vm.heroess.length - 1; i >= 0; i--) {
                    if (vm.heroess[i].TaskPublicKey == msg.taskPk) {
                        vm.heroess[i].Status = 'inProgress';
                        vm.heroess[i].ApprovedToMe = false;
                        return;
                    }
                }
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
            vm.exparedOrder = exparedOrder;
            vm.inDisputOrder = inDisputOrder;
            vm.startWorking = startWorking; 
            vm.completeTask = completeTask;

            function closeOrder(index, orderPk) {
                $resource(urlSvc.MyOrderClose, { pk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is closed', 'success');
                        vm.heroess[index].Status = 'Closed';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }

            function exparedOrder(index, orderPk) {
                $resource(urlSvc.MyOrderReopen, { pk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is expared', 'success');
                        vm.heroess[index].Status = 'Open';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }

            function inDisputOrder(index, orderPk) {
                $resource(urlSvc.MyOrderToDisput, { pk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job is InDispute', 'success');
                        vm.heroess[index].Status = 'InDispute';
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }

            function removeOrder(index, orderPk) {
                sweetAlert.swal({
                    title: 'Are you sure?',
                    text: 'Your will not be able to recover this!',
                    type: 'warning',
                    showCancelButton: true,
                    confirmButtonColor: '#DD6B55',
                    confirmButtonText: 'Yes, delete it!',
                    cancelButtonText: 'Cancel',
                    closeOnConfirm: false,
                    closeOnCancel: false
                }, function (isConfirm) {
                    if (isConfirm) {
                        $resource(urlSvc.MyOrderRemove, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                            console.log(response);
                            if (response.IsSuccess) {
                                vm.heroess.splice(index, 1);
                                sweetAlert.swal('Deleted!', 'Your job has been deleted.', 'success');
                            } else {
                                sweetAlert.swal('Cancelled', 'Some Error', 'error');
                            }
                        });
                    } else {
                        sweetAlert.swal('Cancelled', 'Your job is safe :)', 'error');
                    }
                });
            }

            function startWorking(index, orderPk) {
                $resource(urlSvc.TaskStartWorking, { taskPk: orderPk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        sweetAlert.swal('Approuved!', 'Your job has started', 'success');
                        vm.heroess[index].Status = 'InProgress';
                        vm.heroess[index].ItsMyTask = true;
                    } else {
                        sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                    }
                });
            }
            
            function completeTask(index, orderPk) {
                vm.modalInstance = $uibModal.open({
                    templateUrl: '/commentForCustomer.html',
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
                    if (mark+1 === $scope.rate)
                        $scope.rate = 0;
                    else
                        $scope.rate = mark+1;
                }

                $scope.outFocusFromStar = function (index) {
                    $scope.stars = ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                    for (var i = 0; i <= $scope.rate-1; i++)
                        $scope.stars[i] = 'fa-star';
                }

                $scope.onFocusToStar = function (index) {
                    $scope.stars = ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                    for (var i = 0; i <= index; i++)
                        $scope.stars[i] = 'fa-star';
                }

                $scope.mention = "";

                $scope.ok = function () {
                    $resource(urlSvc.MyOrderClose, { taskPk: orderPk, rate: $scope.rate, mention: $scope.mention }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                        $uibModalInstance.dismiss('cancel');
                        if (response.IsSuccess) {
                            if (response.Status === 'Closed') {
                                sweetAlert.swal('Approuved!', 'Your job has closed', 'success');
                                vm.heroess[index].Status = 'Closed';
                            } else if (response.Status === 'AwaitingCustomerDecision') {
                                sweetAlert.swal('Thank you!', 'Expect to customer decision!', 'success');
                                vm.heroess[index].Status = 'AwaitingCustomerDecision';
                            } 
                        } else {
                            sweetAlert.swal('Cancelled', 'Sorry some error', 'error');
                        }
                    });
                }

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                }

                $scope.generatePositiveReview = function () { $scope.mention = "Работаем с заказчиком уже довольно давно. Никаких нареканий, одни положительные впечатления. Будем продолжать сотрудничество."; }
                $scope.generateNegativeReview = function () { $scope.mention = "Заказчик является самым отявленным негодяеи из всех негодяйских негодяев. Курит, пьёт, а также сквернословит трёхэтажным матом в присутствии детей и женщин."; }
            }

            vm.setNotifiAsViewed = function (pk) {
                $resource(urlSvc.SetTaskNotificationAsViewed, { taskPk: pk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) { });
            }
        }
    }
})();
