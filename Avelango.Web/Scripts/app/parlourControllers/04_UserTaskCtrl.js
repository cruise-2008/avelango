(function () {
    'use strict';

    angular
        .module('app.user.task.controller', [])
        .controller('UserTaskCtrl', userTaskCtrl);

    userTaskCtrl.$inject = ['$timer', '$parlourTicketService', '$http', '$rootScope', '$scope', 'sweetAlert'];
    function userTaskCtrl($timer, $parlourTicketService, $http, $rootScope, $scope, sweetAlert) {

        var vm = this;
        vm.activate = false;
        vm.taskTable = [];

        $("div.item-cart").height($("body").height() - ($('body').height() * 15 / 100));
        $("div.list-block").height($("div.item-cart").height() / 3.1);
        $("div#list-block").height($("div.item-cart").height() / 1.8);

        var changeStatus = function (status) {
            switch (status) {
                case "Open": { return "Открыто"; }
                case "Closed": { return "Закрыто"; }
                case "Expired": { return "Неактуально"; }
                case "Deactivated": { return "Деактивировано"; }
                case "InDispute": { return "Спор"; }
                case "InProgress": { return "В работе"; }
                case "AwaitingModeratorDecision": { return "На проверке"; }
                case "AwaitingCustomerDecision": { return "Выбор исполнителя"; }
                case "AwaitingWorkerDecision1": { return "Ожидание исполнителя"; }
                case "AwaitingWorkerDecision2": { return "Ожидание исполнителя"; }
            }
        };

        
        $scope.$on('userTask', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();


                function activate() {

                    var tasks = [];
                    var filtredTasks = [];
                    var deletedTasks = [];

                    var taskIndex = 0;

                    var arrowLeft = angular.element(document.querySelector("#leftArrow"));
                    var arrowRight = angular.element(document.querySelector("#rightArrow"));
                    var arrowLeftArchive = angular.element(document.querySelector("#leftArrow1"));;
                    var arrowRightArchive = angular.element(document.querySelector("#rightArrow1"));;

                    var showTasks = function (showActive, subtaskArray, viewChanged) {
                        var arrayForShow = [];
                        var lastIndex = 0;

                        if (viewChanged) taskIndex = 0;
                        var counter = subtaskArray.length - taskIndex < vm.numberTasksPerPage ? subtaskArray.length - taskIndex : vm.numberTasksPerPage;

                        for (var i = taskIndex; i < counter + taskIndex; i++) {
                            if (i < tasks.length) {
                                arrayForShow.push(subtaskArray[i]);
                                lastIndex = i + 1;
                            }
                        }

                        if (showActive) {

                            vm.subTasks = arrayForShow;

                        } else {
                            vm.tasksWichIRemoved = arrayForShow;

                            if (lastIndex === tasks.length || lastIndex === subtaskArray.length || subtaskArray.length === 0) {
                                $scope.disabledRight1 = true;
                                arrowRightArchive.addClass("disabledPaginationButton");
                            } else {
                                $scope.disabledRight1 = false;
                                arrowRightArchive.removeClass("disabledPaginationButton");
                            }
                            if (taskIndex === 0 || subtaskArray.length === 0) {
                                $scope.disabledLeft1 = true;
                                arrowLeftArchive.addClass("disabledPaginationButton");
                            } else {
                                $scope.disabledLeft1 = false;
                                arrowLeftArchive.removeClass("disabledPaginationButton");
                            }
                        }


                        if (lastIndex === tasks.length || lastIndex === subtaskArray.length || subtaskArray.length === 0 ) { 
                            $scope.disabledRight = true;
                            arrowRight.addClass("disabledPaginationButton");
                        } else {
                            $scope.disabledRight = false;
                            arrowRight.removeClass("disabledPaginationButton");
                        }
                        if (taskIndex === 0 || subtaskArray.length === 0 ) {
                            $scope.disabledLeft = true;
                            arrowLeft.addClass("disabledPaginationButton");
                        } else {
                            $scope.disabledLeft = false;
                            arrowLeft.removeClass("disabledPaginationButton");
                        }
                    };


                    var stepup = function (showActive) {
                        if (taskIndex < tasks.length) {
                            taskIndex += vm.numberTasksPerPage;

                            vm.subTasks = [];
                            vm.tasksWichIRemoved = [];

                            if (filtredTasks.length > 0) {
                                showTasks(showActive, filtredTasks);
                            } else {
                                if (showActive) {
                                    showTasks(showActive, tasks);
                                } else {
                                    showTasks(showActive, deletedTasks);

                                }
                            }
                        }
                    };

                    vm.stepUp = function () {
                        stepup(true, tasks);
                        vm.currentPage++;
                    };

                    vm.stepUpInArchive = function() {
                        stepup(false, deletedTasks);
                        vm.currentPageForDeletetTasks++;
                    };

                    var stepdown = function (showActive) {
                        if (taskIndex >= vm.numberTasksPerPage) {
                            taskIndex -= vm.numberTasksPerPage;
                            vm.subTasks = [];
                            vm.tasksWichIRemoved = [];
                           
                           if (filtredTasks.length > 0) {
                               showTasks(showActive, filtredTasks);
                           } else {
                               if (showActive) {
                                   showTasks(showActive, tasks);
                               } else {
                                   showTasks(showActive, deletedTasks);

                               }
                           }
                        }
                    };

                    vm.stepDown = function() {
                        stepdown(true, tasks);
                        vm.currentPage--;
                    };

                    vm.stepDownInArchive = function() {
                        stepdown(false, deletedTasks);
                        vm.currentPageForDeletetTasks--;
                    };

                    $scope.showLoader = true;

                    


                    $http({

                        method: "POST",
                        url: "/Task/GetMyOrders"

                    }).success(function (responce) {

                        $scope.showLoader = false;

                        if (responce.IsSuccess) {
                            responce.Orders.forEach(function (item) {
                                if (item.RemovedForCustomer === false) {
                                    tasks.push(item);
                                } else {
                                    deletedTasks.push(item);
                                }
                                item.Checked = false;
                                item.Lineclass = '';
                                item.Created = $timer.getTimetoparlour(item.Created.replace(/\D+/g, ""));
                                item.TopicalTo = $timer.getTimetoparlour(item.TopicalTo.replace(/\D+/g, ""));
                                item.Closed = $timer.getTimetoparlour(item.Closed.replace(/\D+/g, ""));
                                item.Status = changeStatus(item.Status);
                            });
                        }
                        if (tasks.length === 0 ) {
                            vm.currentPage = 0;
                        } else {
                            vm.currentPage = 1;
                        }
                        if (deletedTasks.length === 0) {
                            vm.currentPageForDeletetTasks = 0;
                        } else {
                            vm.currentPageForDeletetTasks = 1;
                        }

                        vm.numberTasksPerPage = $parlourTicketService.getPaginationLenght('trHeight');
                        vm.allPages = Math.ceil(tasks.length / vm.numberTasksPerPage);
                        vm.allPagesForDeletedTasks = Math.ceil(deletedTasks.length / vm.numberTasksPerPage);
                        vm.subTasks = [];
                        vm.tasksWichIRemoved = [];
                        showTasks(vm.subTasks, tasks);

                    }).error(function () {

                        sweetAlert.swal("Ошибка таски в личном кабинете не пришли");
                        $scope.showLoader = false;
                        return;

                    });
                    
                    $scope.disabled = true;

                    var trash = angular.element(document.querySelector("#trash"));

                    $scope.checkinTask = function (task) {

                        if (task.Checked === true) {
                            task.Lineclass = "checkedTask";
                            $scope.disabled = false;
                            trash.removeClass("disabled");
                        } else {
                            task.Lineclass = "";
                        }
                        var checkedExist = false;
                        tasks.forEach(function (v) {
                            if (v.Checked) {
                                checkedExist = true;
                            }
                        });
                        if (!checkedExist) {
                            $scope.disabled = true;
                            trash.addClass("disabled");
                        }
                    };

                    $scope.removedTasks = [];

                    vm.removeTasksToArchive = function () {
                        tasks.forEach(function (v, i) {
                            var checked = v.Checked;
                            if (checked === true) {
                                $scope.removedTasks.push(v.PublicKey);
                            }
                        });
                        $http({
                            method: "POST",
                            url: "/Task/MyOrdersRemove",
                            data: JSON.stringify($scope.removedTasks)
                        }).success(function (responce) {


                            if (responce.IsSuccess) {
                                tasks.forEach(function (v, i) {
                                    var checked = v.Checked;
                                    if (checked === true) {
                                        v.RemovedForCustomer = true;
                                        v.Checked = false;
                                        v.Lineclass = "";
                                        vm.subTasks = [];
                                    }
                                });
                                alert('ggg');
                            }
                        }).error(function () {

                           //$scope.showLoader = false;
                            return;

                        });

                    };


                    $scope.searchTasks = function () {
                        filtredTasks = [];
                        if ($scope.valueInput === "") {
                            showTasks(true, tasks);
                        }
                        tasks.forEach(function (v, i) {

                            var name = v.Name.toLowerCase();
                            var price = v.Price.toString().toLowerCase();
                            var dateCreated = v.Created.d.toLowerCase();
                            var dateTopicalTo = v.TopicalTo.d.toLowerCase();
                            var status = v.Status.toLowerCase();
                            var myInput = $scope.valueInput.toLowerCase();


                            if (~name.indexOf(myInput)) {
                                filtredTasks.push(v);
                            }
                            else if (~price.indexOf(myInput)) {
                                filtredTasks.push(v);
                            }
                            else if (~dateCreated.indexOf(myInput)) {
                                filtredTasks.push(v);
                            }
                            else if (~dateTopicalTo.indexOf(myInput)) {
                                filtredTasks.push(v);
                            }
                            else if (~status.indexOf(myInput)) {
                                filtredTasks.push(v);
                            }
                        });

                        vm.subTasks = [];
                        taskIndex = 0;
                        showTasks(true, filtredTasks);
                        vm.numberTasksPerPage = $parlourTicketService.getPaginationLenght('trHeight');
                        

                        if (filtredTasks.length === 0) {
                            vm.currentPage = 0;
                            vm.allPages = 0;
                        } else {
                            vm.currentPage = 1;
                            vm.allPages = Math.ceil(filtredTasks.length / vm.numberTasksPerPage);
                        }
                    };
                    vm.tasks = {};
                    $scope.clicked = 'Tasks';
                    $scope.changePage = function(con,task) {
                        $scope.clicked = con;
                        vm.task = task;
                        if ($scope.clicked === 'Archive') {
                            vm.tasksWichIRemoved = [];
                            showTasks(false, deletedTasks, true);
                            vm.currentPageForDeletetTasks = 1;
                        } else {
                            vm.subTasks = [];
                            showTasks(true, tasks, true);
                            vm.currentPage = 1;
                        }
                        if (vm.tasksWichIRemoved.length === 0) {
                            vm.currentPageForDeletetTasks = 0;
                        }
                    }

                    $scope.Description = true;

                    $scope.changeTabs = function (step) {

                        document.getElementById("description").classList.remove('active');
                        document.getElementById("comment").classList.remove('active');
                        document.getElementById("executors").classList.remove('active');
                        document.getElementById("descriptionTask").classList.remove('active');
                        document.getElementById("comments").classList.remove('active');
                        document.getElementById("workersToMyJob").classList.remove('active');

                        $scope.Description = false;
                        $scope.Comments = false;
                        $scope.Executor = false;

                        switch (step) {
                            case 'description':
                                $scope.Description = true;
                                if ($scope.Description === true) {
                                    document.getElementById("description").classList.add('active');
                                    document.getElementById("descriptionTask").classList.add('active');
                                }
                                break;
                            case 'comments':
                                $scope.Comments = true;
                                if ($scope.Comments === true) {
                                    document.getElementById("comment").classList.add('active');
                                    document.getElementById("comments").classList.add('active');
                                }
                                break;
                            case 'executor':
                                $scope.Executor = true;
                                if ($scope.Executor === true) {
                                    document.getElementById("executors").classList.add('active');
                                    document.getElementById("workersToMyJob").classList.add('active');
                                }
                                break;
                        }
                    }

                    $scope.sortType = 'Created';


















                    vm.activate = true;
                }
            }
        });

    }
})();