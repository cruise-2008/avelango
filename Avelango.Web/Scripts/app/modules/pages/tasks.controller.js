(function () {
    'use strict';

    var module = angular.module('app.pages');

    module.controller('TaskController', taskController);
    taskController.$inject = ['$map', '$timer', '$taskJobService', '$interval', '$compile', '$resource', '$scope', '$rootScope', '$http', '$window', '$uibModal', 'urlSvc', 'starsSvc']; //,'DatatableUsers'

    function taskController($map, $timer, $taskJobService, $interval, $compile, $resource, $scope, $rootScope, $http, $window, $uibModal, urlSvc, starsSvc) {

        var vm = this;
        var sc = $scope;


        activate();

        function activate() {
            vm.jobs = [];//тут будут таски
            vm.chekedfilters = [];//тут отчеканные фильтра
            vm.justActual = 1;//по умолчанию в фильтрах выбраны ток актуальные
            var tascCont = tasckContrainer.dataset.chekedgroup;//группы из вьюбега (на которые подписан юзер)
            var taskPK = tasckContrainer.dataset.choosenpk;//ид задания которое выбрал юзер до логина
            $scope.height = $window.innerHeight;
            $scope.openWindow = false;
            vm.firstQuery = true;
            $scope.myloading = true;

            /**Take Jobs Tasks include timer**/

            var geolocationSuccess = function (position) {
                var paginationLenght = $taskJobService.getPaginationLenght('advert');
                var jData = JSON.stringify({ viewedTasks: [], subgroups: [], justActual: false, placeLat: position.coords.latitude, placeLng: position.coords.longitude, countOfTasks: paginationLenght });
                $taskJobService.getDataInfo(jData, urlSvc.GetFilteredTasks, geFilteredTaskCallBack);
            }


            var geolocationError = function (error) {
                var paginationLenght = $taskJobService.getPaginationLenght('advert');
                var jData = JSON.stringify({ viewedTasks: [], subgroups: [], justActual: false, placeLat: null, placeLng: null, countOfTasks: paginationLenght });
                $taskJobService.getDataInfo(jData, urlSvc.GetFilteredTasks, geFilteredTaskCallBack);
            }


            vm.showByIds = function (job) {
                var shouldBeShowed = false;
                for (var j = 0; j < vm.ids.length; j++) {
                    if (job.id == vm.ids[j]) {
                        shouldBeShowed = true;
                        job.showItems = true;
                        $scope.myloading = false;
                    }
                }
                return shouldBeShowed;
            }

            var changePagenationButtons = function (numbers) {
                vm.pagenationButton1 = numbers[0];
                vm.pagenationButton2 = numbers[1];
                vm.pagenationButton3 = numbers[2];
                vm.pagenationButton4 = numbers[3];
            }


            vm.stepForward = function () {
                var obj = $taskJobService.stepForward();
                if (!$.isEmptyObject(obj.getNewDatas)) {
                    var jData = JSON.stringify({ viewedTasks: obj.getNewDatas.allPubKeys, subgroups: [], justActual: false, placeLat: obj.getNewDatas.lat, placeLng: obj.getNewDatas.lon, countOfTasks: obj.getNewDatas.numberPerPage * 5 });
                    $taskJobService.getDataInfo(jData, urlSvc.GetFilteredTasks, function (data) {
                        var maxId = $taskJobService.getLasrIdNumber();
                        for (var i = 0; i < data.data.Jobs.length; i++) {
                            data.data.Jobs[i].id = "id" + ++maxId;
                        }
                        vm.jobs = vm.jobs.concat(data.data.Jobs);
                        $taskJobService.getReloadPaginationData(vm.jobs);
                    });
                }
                vm.ids = obj.ids;
                changePagenationButtons(obj.buttonNumbers);
            }

            vm.stepBack = function () {
                var obj = $taskJobService.stepBack();
                vm.ids = obj.ids;
                changePagenationButtons(obj.buttonNumbers);
            }

            vm.stepByCount = function (num) {
                var obj = $taskJobService.stepByCount(num);
                vm.ids = obj.ids;
                if (!$.isEmptyObject(obj.startGetNewDatas)) {
                    var jData = JSON.stringify({ viewedTasks: obj.startGetNewDatas.allPubKeys, subgroups: [], justActual: false, placeLat: obj.startGetNewDatas.lat, placeLng: obj.startGetNewDatas.lon, countOfTasks: obj.startGetNewDatas.numberPerPage * 5 });
                    $taskJobService.getDataInfo(jData, urlSvc.GetFilteredTasks, function (data) {
                        var maxId = $taskJobService.getLasrIdNumber();
                        for (var i = 0; i < data.data.Jobs.length; i++) {
                            data.data.Jobs[i].id = "id" + ++maxId;
                        }
                        vm.jobs = vm.jobs.concat(data.data.Jobs);
                        $taskJobService.getReloadPaginationData(vm.jobs);
                    });
                }
                changePagenationButtons(obj.buttonNumbers);
            }

            var startTimer = function (paginationData) {
                for (var i = 0; i < paginationData.length; i++) {
                    var topicalObj = $timer.getTopicalTo(paginationData[i].TopicalTo);
                    paginationData[i].days = topicalObj.d
                    paginationData[i].hours = topicalObj.h
                    paginationData[i].minutes = topicalObj.m;
                    paginationData[i].show = topicalObj.d != 0 && topicalObj.h != 0 && topicalObj.m != 0;
                }
                return paginationData;
                return interval();
            }


            var interval = $interval(function () {
                for (var j = 0; j < vm.jobs.length; j++) {
                    if (vm.jobs[j].hours == 0 && vm.jobs[j].days == 0 && vm.jobs[j].minutes == 0) {
                        vm.jobs[j].show = false;
                    }
                    else {
                        if (vm.jobs[j].minutes == 0) {
                            if (vm.jobs[j].hours == 0 && vm.jobs[j].days == 0) {
                                vm.jobs[j].minutes = 0;
                            }
                            else {
                                vm.jobs[j].minutes = 59;
                                vm.jobs[j].hours--;
                            }
                        }
                        else {
                            if (vm.jobs[j].hours == 0) {
                                if (vm.jobs[j].days == 0) {
                                    vm.jobs[j].hours = 0;
                                    vm.jobs[j].minutes--;
                                }
                                else {
                                    vm.jobs[j].hours = 23;
                                    vm.jobs[j].days--;
                                }
                            }
                            else {
                                vm.jobs[j].minutes--;
                            }
                        }
                    }
                }
            }, 60000, 0);


            var geFilteredTaskCallBack = function (response) {
                if (response.data.IsSuccess) {
                    for (var j = 0; j < response.data.Jobs.length; j++) {
                        response.data.Jobs[j].showItems = true;
                        response.data.Jobs[j].id = "id" + j;
                    }
                    vm.jobs = response.data.Jobs;

                    var obj = $taskJobService.getPaginationData(vm.jobs);
                    vm.ids = obj.ids;
                    changePagenationButtons(obj.buttonNumbers);
                    startTimer(response.data.Jobs);
                }
            }


            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(geolocationSuccess, geolocationError);
            }
            var autocomplite = $map.autocomplite('search');
            //**End Take Jobs Tasks include timer**//






            //** Take Current Jobs Task **//

            sc.taskData;
            sc.openTaskWindow = true;

            var getTaskData = function (PublicKey) {
                var jPk = JSON.stringify({ pk: PublicKey });

                $http({
                    method: 'POST',
                    data: jPk,
                    url: '/Task/GetTask',
                }).then(function (response) {
                    sc.taskData = response.data.Job[0];
                    sc.timeData = $timer.getTopicalTo(response.data.Job[0].TopicalTo);
                    sc.timeData.show = sc.timeData.d != 0 && sc.timeData.h != 0 && sc.timeData.m != 0;

                });
                return {};
            }
            $("div.side-world").height($("body").height() - ($('body').height() * 20 / 100));

            vm.showTaskContent = function (PublicKey) {

                if (sc.taskData == undefined) {
                    $http({
                        method: 'GET',
                        url: '/Task/TaskCard',
                    }).success(function (response) {
                        $('#ticketContent').append($compile(response)(sc));
                        sc.openTaskWindow = false;
                        sc.taskData = getTaskData(PublicKey);
                        $("div.item-cart").height($("body").height() - ($('body').height() * 23 / 100));
                        $("div#listblockopentask").height($("div.item-cart").height() / 4.4);
                    }).error(function () {
                        alert("error");
                        return null;
                    });
                } else {
                    sc.openTaskWindow = false;
                    sc.taskData = getTaskData(PublicKey);
                }
            };


            $rootScope.closeCurentJob = function () {
                sc.openTaskWindow = true;
            };



            $scope.AboutProject = true;

            $scope.changeTabsOpenTask = function (step) {

                document.getElementById("resume").classList.remove('active');
                document.getElementById("descriptionopentask").classList.remove('active');
                document.getElementById("comments").classList.remove('active');
                document.getElementById("commentopentask").classList.remove('active');

                $scope.AboutProject = false;
                $scope.Comments = false;

                switch (step) {
                    case 'aboutProject':
                        $scope.AboutProject = true;
                        if ($scope.AboutProject === true) {
                            document.getElementById("resume").classList.add('active');
                            document.getElementById("descriptionopentask").classList.add('active');
                        }
                        break;
                    case 'comments':
                        $scope.Comments = true;
                        if ($scope.Comments === true) {
                            document.getElementById("comments").classList.add('active');
                            document.getElementById("commentopentask").classList.add('active');
                        }
                        break;
                }
            }

            //**End Current Jobs Task **//







            //// var getMyGropFunction = function () {
            ////     $resource(urlSvc.getMyGroups, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
            ////         if (response.IsSuccess) {
            ////             if (response.Subscribed) {
            ////                 vm.chekedGroups = JSON.parse(response.Subscribed);
            ////                 var groups = [];
            ////                 angular.forEach(response.Groups, function (value, key) {
            ////                     var self = this;
            ////                     var nv = function newvalue(value) {
            ////                         if (!tascCont) {
            ////                             value.SubGroups.forEach(function (item, i, arr) {
            ////                                 if (vm.chekedGroups.indexOf(item.Name) >= 0) {
            ////                                     value.SubGroups[i].Cheked = true;
            ////                                 }
            ////                             });
            ////                         }
            ////                         return value;
            ////                     }
            ////                     self.push(nv(value));
            ////                 }, groups);
            ////
            ////                 vm.groups = groups;
            ////                 vm.getFilteredTasks('undefined');
            ////             } else {
            ////                 vm.groups = response.Groups;
            ////                 ////tascCont ? vm.getFilteredTasks(tascCont) : vm.getFilteredTasks('undefined');
            ////             }
            ////             ////vm.checkFilter();
            ////         }
            ////     });
            //// }
            ////
            //// var myplace = {};
            //// if (navigator.geolocation) {
            ////     navigator.geolocation.getCurrentPosition(function (position) {
            ////         myplace = position;
            ////         vm.pyrmont = [myplace.coords.latitude, myplace.coords.longitude];
            ////         //getMyGropFunction();
            ////         $taskJobService.setPlace(myplace.coords.latitude, myplace.coords.longitude);
            ////     });
            //// } else {
            ////     vm.pyrmont = [null, null];
            ////     // getMyGropFunction();
            //// }




            vm.expandImage = function (id) {//Развернуть картинку
                vm.modalInstance = $uibModal.open({
                    templateUrl: '/imageTaskModal.html',
                    controller: ModalCtrl,
                    resolve: {
                        title: function () { return ""; },
                        price: function () { return ""; },
                        showedImgId: function () { return id; },
                        showedImgTitle: function () { return vm.images[id].Title; },
                        showedImgUrl: function () { return vm.images[id].Url; }
                    }
                });
            }

            vm.bidTask = function (orderTitle, orderPrice, orderPk) { //берем задание шаг1
                vm.orderTitle = orderTitle;
                vm.orderPrice = orderPrice;
                vm.orderPk = orderPk;

                vm.modalInstance1 = $uibModal.open({
                    templateUrl: '/bidTaskModal.html',
                    controller: ModalCtrl,
                    resolve: {
                        title: function () { return orderTitle; },
                        price: function () { return orderPrice; },
                        showedImgTitle: function () { return ""; },
                        showedImgUrl: function () { return ""; },
                        showedImgId: function () { return ""; }
                    }
                });
            }

            ModalCtrl.$inject = ['$scope', '$uibModalInstance', '$rootScope', 'title', 'price', 'sweetAlert', 'showedImgUrl', 'showedImgTitle', 'showedImgId', 'urlSvc']; //контроллер модального окна для картинок и для того, чтоб взять задание- разделить
            function ModalCtrl($scope, $uibModalInstance, $rootScope, title, price, sweetAlert, showedImgUrl, showedImgTitle, showedImgId, urlSvc) {

                $scope.title = title;
                $scope.price = price;
                $scope.showedImgTitle = showedImgTitle;
                $scope.showedImgUrl = showedImgUrl;
                $scope.showedImgId = showedImgId;
                $scope.messageForBid = "";

                $scope.cancel = function () {
                    $uibModalInstance.dismiss('cancel');
                };

                $scope.previewOrNextImage = function (idx) { //для пролистывания картинок
                    if (idx === -1) {
                        idx = vm.images.length - 1;
                    } else if (idx === vm.images.length) {
                        idx = 0;
                    }
                    $uibModalInstance.dismiss('cancel');
                    vm.expandImage(idx);
                };

                $scope.bidTaskRequest = function () { //берем задание шаг2
                    var jData = JSON.stringify({
                        'taskPk': vm.orderPk,
                        'message': $scope.messageForBid,
                        'price': $scope.price
                    });
                    var req = {
                        url: urlSvc.BidTask,
                        method: 'POST',
                        data: jData,
                        headers: { 'Content-Type': "application/json" }
                    };

                    $http(req).then(function (response) {
                        if (!response.data.IsSuccess) {
                            sweetAlert.swal('Error!', 'Some server error', 'error');
                        } else {
                            $uibModalInstance.dismiss('cancel');
                            vm.jobs[vm.ind].AlreadyBided = true;
                            sweetAlert.swal('Good job!', 'The task is selected', 'success');
                        }
                    }, function () { });
                }
            }

            angular.element($window).bind('resize', function () {// ?
                $scope.height = $window.innerHeight;
                $scope.$digest();
            });

            vm.checkFilter = function (gr, sGr) {  //для отображения выбранных фильтров в виде тикетов
                if (gr !== undefined && sGr !== undefined) {
                    if (vm.groups[gr].SubGroups[sGr].Cheked) {
                        var newItem = { Name: vm.groups[gr].SubGroups[sGr].Name, GroupId: gr, SubgroupId: sGr }
                        vm.chekedfilters.push(newItem);
                    } else {
                        for (var i = 0; i < vm.chekedfilters.length; i++)
                            if (vm.chekedfilters[i].GroupId === gr && vm.chekedfilters[i].SubgroupId === sGr)
                                vm.chekedfilters.splice(i, 1);
                    }
                } else {  //для того случая, когда ф-я вызывается не при изменении чекбокса, а при первом запуске, когда по умолчанию уже могут быть выбраны фильтры
                    var chekedfilters2 = [];
                    angular.forEach(vm.groups, function (value, index) {
                        var self = this;
                        value.SubGroups.forEach(function (item, i, arr) {
                            if (item.Cheked === true) {
                                var newItem = { Name: item.Name, GroupId: index, SubgroupId: i }
                                chekedfilters2.push(newItem);
                                vm.chekedfilters = chekedfilters2;
                            }
                        });
                    });
                }
            }

            vm.removeFilter = function (gr, sGr) {
                vm.groups[gr].SubGroups[sGr].Cheked = false;
                for (var i = 0; i < vm.chekedfilters.length; i++)
                    if (vm.chekedfilters[i].GroupId === gr && vm.chekedfilters[i].SubgroupId === sGr)
                        vm.chekedfilters.splice(i, 1);
            }

            function checkGroup(chekedfiltergroups) {
                ////angular.forEach(vm.groups, function (value, key) {
                ////    var self = this;
                ////    value.SubGroups.forEach(function (item, i, arr) {
                ////        if (item.Cheked === true) {
                ////            self.push(item.Name);
                ////        }
                ////    });
                ////}, chekedfiltergroups);
            };


            var justActualForMoreTasks;//эта фигня ниже для того, чтобы не применялись новые категории после нажатия "показать больше" (num == 'getmoretasks'), то есть чтобы категории не перемешивались как то так
            var chekedfiltergroupsForMoreTasks;//
            var pyrmontForMoreTasks;//

            vm.getFilteredTasks = function (num) {
                if (!taskPK) {
                    var chekedfiltergroups = [];
                    var viewedTasks = [];
                    var datas;

                    if (num == 'undefined') {

                        tascCont ? chekedfiltergroups.push(tascCont) : checkGroup(chekedfiltergroups);
                        vm.j = 0;
                        datas = getdatas([], chekedfiltergroups, true, vm.pyrmont);
                        chekedfiltergroupsForMoreTasks = chekedfiltergroups;
                        justActualForMoreTasks = true;
                        pyrmontForMoreTasks = vm.pyrmont;//на самом деле пока бессмыссленно, но в будущем может быть пригодится

                    } else if (num == 'getmoretasks') {

                        angular.forEach(vm.jobs, function (value, key) {
                            viewedTasks.push(value.PublicKey);
                        });
                        datas = getdatas(viewedTasks, chekedfiltergroupsForMoreTasks, justActualForMoreTasks === 1 ? true : false, pyrmontForMoreTasks);

                    } else {

                        checkGroup(chekedfiltergroups);
                        vm.jobs = [];
                        vm.j = 0;
                        datas = getdatas([], chekedfiltergroups, vm.justActual === 1 ? true : false, vm.pyrmont);
                        chekedfiltergroupsForMoreTasks = chekedfiltergroups;
                        justActualForMoreTasks = vm.justActual;
                        pyrmontForMoreTasks = vm.pyrmont;

                    }

                    var jData = JSON.stringify(datas);
                    // List<Guid> viewedTasks, List<string> subgroups, bool justActual, double? placeLat, double placeLng, int countOfTasks

                    $http({ url: urlSvc.GetFilteredTasks, method: 'POST', data: jData }).then(function (response) {
                        if (response.data.IsSuccess) {
                            var jobss = response.data.Jobs;
                            jobss = jobss.map(function (item) {
                                var newitem = item;
                                newitem.TopicalTo = item.TopicalTo.replace(/\D+/g, '');
                                newitem.Customer.Rating = Math.round(item.Customer.Rating * 2) / 2;
                                newitem.Customer.RatingMas = starsSvc.ratingToMass(newitem.Customer.Rating);
                                newitem.Description = {
                                    Description: item.Description,
                                    ShortDescription: item.Description.length > 155 ? item.Description.substr(0, 155) + "..." : item.Description
                                }
                                return newitem;
                            });

                            vm.jobs = vm.jobs.concat(jobss);

                            var j = vm.jobs;

                            if (vm.firstQuery) // 
                                vm.viewedJobs = j.length > vm.dataItemsPerPage ? j.slice(0, vm.dataItemsPerPage) : j.slice(0, j.length);// пачка тикетов отображаемых на одной страничке

                            vm.showValidatemessage = false;
                        }
                    });
                } else {
                    var jPk = JSON.stringify({ pk: taskPK });
                    $http({ url: urlSvc.GetTask, method: 'POST', data: jPk }).then(function (response) {
                        if (response.data.IsSuccess) {
                            var jobss = response.data.Job;
                            jobss = jobss.map(function (item) {
                                var newitem = item;
                                newitem.TopicalTo = item.TopicalTo.replace(/\D+/g, '');
                                newitem.Customer.Rating = Math.round(item.Customer.Rating * 2) / 2;
                                newitem.Customer.RatingMas = starsSvc.ratingToMass(newitem.Customer.Rating);
                                newitem.Description = {
                                    Description: item.Description,
                                    ShortDescription: item.Description.length > 155 ? item.Description.substr(0, 155) + "..." : item.Description
                                }
                                return newitem;
                            });

                            vm.jobs = vm.jobs.concat(jobss);

                            var j = vm.jobs;

                            var j = vm.jobs;

                            if (vm.firstQuery) // 
                                vm.viewedJobs = j.length > vm.dataItemsPerPage ? j.slice(0, vm.dataItemsPerPage) : j.slice(0, j.length);// пачка тикетов отображаемых на одной страничке

                            vm.showValidatemessage = false;
                        }
                    });
                    tasckContrainer.dataset.choosenpk = null;
                    taskPK = null;
                }
            };

            vm.hidebuttonMoreTasks = function () {
                vm.buttonMoreJob = false;
            }

            var urlPath = window.location.pathname;
            window.history.pushState("", "", urlPath);

            vm.closeCurentJob = function () {
                $scope.openWindow = false;
            }

            vm.openCurentJob = function (index) {
                $scope.ind = index;
                vm.ind = index;
                vm.images = [];
                vm.otherFiles = [];
                var imageExtentions = [".png", ".jpg", ".tif", ".gif", ".bmp", ".PNG", ".JPG"];
                vm.viewedJobs[index].Attachments.forEach(function (v, i) {
                    if (imageExtentions.indexOf(v.Extention) > -1) {
                        vm.images.push({ Url: v.Url, Title: v.FileTitle });
                    } else {
                        vm.otherFiles.push({ Url: v.Url, Title: v.FileTitle });
                    }
                });
                $scope.openWindow = true;
            };

            vm.pagination = function (pageNumber) {
                var j = vm.jobs;

                if (vm.jobs.length > (pageNumber - 1) * vm.dataItemsPerPage + vm.dataItemsPerPage)
                    vm.viewedJobs = j.slice((pageNumber - 1) * vm.dataItemsPerPage, ((pageNumber - 1) * vm.dataItemsPerPage) + vm.dataItemsPerPage);
                else {
                    vm.viewedJobs = j.slice((pageNumber - 1) * vm.dataItemsPerPage, vm.jobs.length);
                    vm.firstQuery = false;
                    vm.getFilteredTasks('getmoretasks');
                }

            }
        }
    }
})();
