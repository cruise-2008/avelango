/**=========================================================
 * Module: pages,js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.pages')
        .controller('ExecutorsController', executorsController);

    executorsController.$inject = ['$map', '$taskJobService' ,'$compile', '$uibModal', '$resource', '$scope', '$rootScope', '$http', '$window', 'sweetAlert', 'urlSvc', 'starsSvc'];//,'DatatableUsers'
    function executorsController($map ,$taskJobService ,$compile ,$uibModal, $resource, $scope, $rootScope, $http, $window, sweetAlert, urlSvc, starsSvc) {
        
        var vm = this;
        var sc = $scope;

        activate();

        function activate() {
            vm.isOnline = false;
            vm.jobs = [];
            vm.chekedfilters = [];
            $scope.myloadingexecutors = true;


            var geolocationSuccess = function (position) {
                var paginationLenght = $taskJobService.getPaginationLenght('person');
                var jData = JSON.stringify({ viewedUsers: [], subgroups: [], isOnline: false, placeLat: null, placeLng: null, countOfUsers: paginationLenght });
                $taskJobService.getDataInfo(jData, urlSvc.GetFilteredUsers, geFilteredExecutorsCallBack);
            }


            var geolocationError = function (error) {
                var paginationLenght = $taskJobService.getPaginationLenght('person');
                var jData = JSON.stringify({ viewedUsers: [], subgroups: [], isOnline: false, placeLat: null, placeLng: null, countOfUsers: paginationLenght });
                $taskJobService.getDataInfo(jData, urlSvc.GetFilteredUsers, geFilteredExecutorsCallBack);
            }


            vm.showByIds = function (job) {
                var shouldBeShowed = false;
                for (var j = 0; j < vm.ids.length; j++) {
                    if (job.id == vm.ids[j]) {
                        shouldBeShowed = true;
                        job.showItems = true;
                        $scope.myloadingexecutors = false;
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
                    var jData = JSON.stringify({ viewedUsers: obj.getNewDatas.allPubKeys, subgroups: [], isOnline: false, placeLat: obj.getNewDatas.lat, placeLng: obj.getNewDatas.lon, countOfUsers: obj.getNewDatas.numberPerPage * 5 });
                    $taskJobService.getDataInfo(jData, urlSvc.GetFilteredUsers, function (data) {
                        var maxId = $taskJobService.getLasrIdNumber();
                        for (var i = 0; i < data.data.Users.Data.length; i++) {
                            data.data.Users.Data[i].id = "id" + ++maxId;
                        }
                        vm.jobs = vm.jobs.concat(data.data.Users.Data);
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
                    var jData = JSON.stringify({ viewedUsers: obj.startGetNewDatas.allPubKeys, subgroups: [], isOnline: false, placeLat: obj.startGetNewDatas.lat, placeLng: obj.startGetNewDatas.lon, countOfUsers: obj.startGetNewDatas.numberPerPage * 5 });
                    $taskJobService.getDataInfo(jData, urlSvc.GetFilteredTasks, function (data) {
                        var maxId = $taskJobService.getLasrIdNumber();
                        for (var i = 0; i < data.data.Users.Data.length; i++) {
                            data.data.Users[i].id = "id" + ++maxId;
                        }
                        vm.jobs = vm.jobs.concat(data.data.Users.Data);
                        $taskJobService.getReloadPaginationData(vm.jobs);
                    });
                }
                changePagenationButtons(obj.buttonNumbers);
            }



            var geFilteredExecutorsCallBack = function (response) {
                if (response.data.IsSuccess) {
                    for (var j = 0; j < response.data.Users.Data.length; j++) {
                        response.data.Users.Data[j].showItems = true;
                        response.data.Users.Data[j].id = "id" + j;
                    }
                    vm.jobs = response.data.Users.Data;

                    var obj = $taskJobService.getPaginationData(vm.jobs);
                    vm.ids = obj.ids;
                    changePagenationButtons(obj.buttonNumbers);
                }
            }


            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(geolocationSuccess, geolocationError);
            }



            //**Take Current Executors Data**//

            $scope.executorsData;
            $scope.openExecutorsWindow = true;

            var getExecutorsData = function (Pk) {
                var jPk = JSON.stringify({ userPk: Pk });
           
                $http({
                    method: 'POST',
                    data: jPk,
                    url: '/Users/GetFullUserInfo',
                }).then(function (response) {
                    $scope.executorsData = response.data.User;
                });
                return {};
            }


            vm.showExecutorsContent = function (Pk) {

                if ($scope.executorsData == undefined) {
                    $http({
                        method: 'GET',
                        url: '/Users/ExecutorsCard',
                    }).success(function (response) {
                        $('#executorContent').append($compile(response)($scope));
                        $scope.openExecutorsWindow = false;
                        $scope.executorsData = getExecutorsData(Pk);
                    }).error(function () {
                        alert("error");
                        return null;
                    });
                } else {
                    $scope.openExecutorsWindow = false;
                    $scope.executorsData = getExecutorsData(Pk);
                }
            };

            $rootScope.closeCurentExecutor = function () {
                $scope.openExecutorsWindow = true;
            }


            $scope.Aboutme = true;

            $scope.changeExecutorOpenPage = function (step) {

                document.getElementById("openexecutor").classList.remove('active');
                document.getElementById("resumeexecutor").classList.remove('active');
                document.getElementById("openportfolioexecutor").classList.remove('active');
                document.getElementById("portfolioexecutor").classList.remove('active');
                document.getElementById("openotziviexecutor").classList.remove('active');
                document.getElementById("commentsexecutor").classList.remove('active');
                

                $scope.Aboutme = false;
                $scope.Portfolio = false;
                $scope.Otzivi = false;

                switch (step) {
                    case 'aboutexecutor':
                        $scope.Aboutme = true;
                        if ($scope.Aboutme === true) {
                            document.getElementById("openexecutor").classList.add('active');
                            document.getElementById("resumeexecutor").classList.add('active');
                        }
                        break;
                    case 'portfolio':
                        $scope.Portfolio = true;
                        if ($scope.Portfolio === true) {
                            document.getElementById("openportfolioexecutor").classList.add('active');
                            document.getElementById("portfolioexecutor").classList.add('active');
                        }
                        break;
                    case 'otzivi':
                        $scope.Otzivi = true;
                        if ($scope.Otzivi === true) {
                            document.getElementById("openotziviexecutor").classList.add('active');
                            document.getElementById("commentsexecutor").classList.add('active');
                        }
                        break;
                }
            }











            //**End Current Executors Data **//


            // $http({
            //     method: 'GET',
            //     url: '/Users/ExecutorsCard',
            // }).success(function (response) {
            //     $('#executorContent').append($compile(response)(sc));
            //     sc.openExecutorsWindow = true;
            //     sc.ExecutorsData = true;
            //
            // }).error(function () {
            //     alert("error");
            //     return null;
            // });








































































            // var myAutocomplite = function () {
            //     var inputFrom = document.getElementById('search');
            //     var autocompleteFrom = new google.maps.places.Autocomplete(inputFrom, {});
            //     google.maps.event.addListener(autocompleteFrom, 'place_changed', function () {
            //         vm.showValidatemessage = false;
            //         var place = autocompleteFrom.getPlace();
            //         var pyrmont = { lat: place.geometry.location.lat(), lng: place.geometry.location.lng() }
            //         var service = new google.maps.places.PlacesService(document.getElementById('search'));
            //         var request = {
            //             location: pyrmont,
            //             radius: '500',
            //             types: ['regions']
            //         };
            //         var callback = function (results, status) {
            //             vm.plcs = [];
            //             if (status == google.maps.places.PlacesServiceStatus.OK) {
            //                 for (var i = 0; i < results.length; i++) { vm.plcs[i] = results[i].id; }
            //             }
            //         };
            //         service.nearbySearch(request, callback);
            //     });
            // }

            //var interval = setInterval(function () {//это нужно для того чтобы функция myAutocomplite, использующая библиотеку гугл не выполнялась до загрузки этой библиотеки
            //    if (google != 'undefined') {
            //        myAutocomplite();
            //        clearInterval(interval);
            //    }
            //}, 100);

            //vm.myValidation = function () {//валидация для поля фильтрации по локации
            //    vm.plcs = null;
            //    vm.showValidatemessage = true;
            //}






           //// $resource(urlSvc.getMyGroups, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function(response) {
           ////     if (response.IsSuccess) {
           ////         if (response.Subscribed) {
           ////             vm.chekedGroups = JSON.parse(response.Subscribed);
           ////             var groups = [];
           ////             angular.forEach(response.Groups, function (value, key) {
           ////                 var self = this;
           ////                 var nv = function newvalue(value) {
           ////                     value.SubGroups.forEach(function (item, i, arr) {
           ////                         if (vm.chekedGroups.indexOf(item.Name) >= 0) {
           ////                             value.SubGroups[i].Cheked = true;
           ////                         }
           ////                     });
           ////                     return value;
           ////                 }
           ////                 self.push(nv(value));
           ////             }, groups);
           ////             vm.groups = groups;
           ////             vm.getFilteredTasks('undefined');
           ////         } else {
           ////             vm.groups = response.Groups;
           ////             vm.getFilteredTasks('undefined');
           ////         }
           ////         vm.checkFilter();
           ////     }
           //// });




            //**Get Location from Google Maps **//

            var autocomplite = $map.autocomplite('search');

            //**End Get Location from Google Maps**//
            



            vm.checkFilter = function (gr, sGr) {
                if (gr !== undefined && sGr !== undefined) {
                    if (vm.groups[gr].SubGroups[sGr].Cheked) {
                        var newItem = { Name: vm.groups[gr].SubGroups[sGr].Name, GroupId: gr, SubgroupId: sGr }
                        vm.chekedfilters.push(newItem);
                    } else {
                        for (var i = 0; i < vm.chekedfilters.length; i++)
                            if (vm.chekedfilters[i].GroupId === gr && vm.chekedfilters[i].SubgroupId === sGr)
                                vm.chekedfilters.splice(i, 1);
                    }
                } else {//для того случая, когда ф-я вызывается не при изменении чекбокса, а при первом запуски, когда по умолчанию уже могут быть выбраны фильтры
                    var chekedfilters2 = [];
                    angular.forEach(vm.groups, function (value, index) {
                        var self = this;
                        value.SubGroups.forEach(function (item, i, arr) {
                            if (item.Cheked === true) {
                                var newItem = { Name: item.Name, GroupId: index, SubgroupId: i }
                                chekedfilters2.push(newItem);
                                vm.chekedfilters = chekedfilters2;
                               // vm.chekedfilters.push(newItem);
                            }
                        });
                    });
                }
            };

            vm.removeFilter = function (gr, sGr) {
                vm.groups[gr].SubGroups[sGr].Cheked = false;
                for (var i = 0; i < vm.chekedfilters.length; i++)
                    if (vm.chekedfilters[i].GroupId === gr && vm.chekedfilters[i].SubgroupId === sGr)
                        vm.chekedfilters.splice(i, 1);
            }

                var isOnlineForMoreUsers ;//эта фигня и ниже для того, чтобы не применялись новые категории после нажатия "показать больше" (num == 'getmoretasks')
                var chekedfiltergroupsForMoreUsers;//
                var plscForMoreUsers;//

            vm.getFilteredTasks = function (num) {
                var chekedfiltergroups = [];
                var viewedUsers = [];
                var datas;

                if (num == 'undefined') {
                    checkGroup(chekedfiltergroups);
                    vm.j = 0;
                    datas = getdatas([], chekedfiltergroups, vm.plcs, true);
                    isOnlineForMoreUsers = true;
                    chekedfiltergroupsForMoreUsers = chekedfiltergroups;
                } else if (num == 'getmoretasks') {
                    checkGroup(chekedfiltergroups);
                    angular.forEach(vm.jobs, function (value, key) {
                        viewedUsers.push(value.PublicKey);
                    });
                    datas = getdatas(viewedUsers, chekedfiltergroupsForMoreUsers, plscForMoreUsers, isOnlineForMoreUsers);
                } else {
                    vm.jobs = [];
                    vm.j = 0;
                    checkGroup(chekedfiltergroups);
                    datas = getdatas([], chekedfiltergroups, vm.plcs, vm.isOnline);
                    isOnlineForMoreUsers = vm.isOnline;
                    chekedfiltergroupsForMoreUsers = chekedfiltergroups;
                    plscForMoreUsers = vm.plcs;
                }

                var jData = JSON.stringify(datas);

                $http({ url: urlSvc.GetFilteredUsers, method: 'POST', data: jData }).then(function (response) {
                    if (response.data.IsSuccess) {
                        var jobss = response.data.Users.Data;
                        jobss = jobss.map(function (item) {
                            var newitem = item;
                            newitem.Rating = Math.round(item.Rating * 2) / 2;
                            newitem.RatingMas = starsSvc.ratingToMass(newitem.Rating);
                            return newitem;
                        });
                        vm.jobs = vm.jobs.concat(jobss);

                        if (response.data.Users.Data.length < 1)
                            $scope.openWindow = false;
                        else
                            vm.openCurentJob(0);
                    }
                }, function (data) {
                    alert(data);
                });
            };

            function getdatas(vT, sg, plcs, iO) {
                return {
                    viewedUsers: vT,
                    subgroups: sg || [],
                    places: plcs,
                    isOnline: iO
                };
            };

            function checkGroup(chekedfiltergroups) {
                angular.forEach(vm.groups, function (value, key) {
                    var self = this;
                    value.SubGroups.forEach(function (item, i, arr) {
                        if (item.Cheked === true) {
                            self.push(item.Name);
                        }
                    });
                }, chekedfiltergroups);
            };

            vm.openCurentJob = function (index) {
                $scope.ind = index;
                $scope.openWindow = true;
            };

            vm.hidebuttonMoreTasks = function () {
                vm.buttonMoreJob = false;
            }

            vm.getMyOpenTasks = function () {
                $resource(urlSvc.GetMyOpenedTasks, {}, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                    if (response.IsSuccess) {
                        vm.modalInstance = $uibModal.open({
                            templateUrl: '/proposeTaskModal.html',
                            controller: ModalCtrl,
                            resolve: {
                                myOpenTasks: function () {
                                    response.Tasks = response.Tasks.map(function (item) {
                                        var newitem = item;
                                        newitem.Created = item.Created.replace(/\D+/g, '');
                                        return newitem;
                                    });
                                    return response.Tasks;
                                },
                                ind: function() { return $scope.ind; }
                            }
                        });
                    }
                });
            }

            ModalCtrl.$inject = ['$scope', '$uibModalInstance', '$rootScope','sweetAlert', 'myOpenTasks','ind','urlSvc']; 
            function ModalCtrl($scope, $uibModalInstance, $rootScope, sweetAlert, myOpenTasks, ind, urlSvc) {
                $scope.myOpenTasks = myOpenTasks;
                $scope.myOpenChekedTasks = [];

                $scope.ok = function () {
                    var list = [];
                    for (var i = 0; i < $scope.myOpenChekedTasks.length; ++i)
                       if ($scope.myOpenChekedTasks[i])
                           list.push($scope.myOpenTasks[i].PublicKey);

                    $resource(urlSvc.SetOffers, { tasksPk: list, toUser: vm.jobs[ind].Pk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) {
                        sweetAlert.swal('Good job!', 'Good job!', 'success');
                        $uibModalInstance.dismiss('cancel');
                    });
                }
            }

        }
    }
})();
