/**=========================================================
 * Module: .js
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.pages')
        .controller('MainModeratorController', MainModeratorController);

    MainModeratorController.$inject = ['$scope', '$rootScope', '$http', 'urlSvc'];
    function MainModeratorController($scope, $rootScope, $http, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            vm.dataForModerators = [];
            vm.sendMessage = {};

            vm.openMessage = openMessage;

           $rootScope.$on('taskToBusy', function (e, message) {
                vm.dataForModerators = vm.dataForModerators.filter(function (item) {
                    return item.PublicKey !== message;
                });
           });

            $rootScope.$on('userToBusy', function (e, message) {
                vm.usersForModerators = vm.usersForModerators.filter(function (item) {
                    return item.Pk !== message;
                });
            });

            $rootScope.$on('$locationChangeSuccess', function () {
                vm.groups = JSON.parse(main.getAttribute('data-groups'));
            });

            $rootScope.$on('newTaskForModer', function (e, taskPk) {
                $http.post(urlSvc.GetTask, JSON.stringify({ pk: taskPk })).then(function (response) {
                    if (response.data.IsSuccess) {
                        var newTask = response.data.Job[0];
                        newTask.TopicalTo = newTask.TopicalTo.replace(/\D+/g, '');
                        newTask.Created = newTask.Created.replace(/\D+/g, '');
                        var temp;
                        for (var i = vm.dataForModerators.length - 1; i >= 0; i--) {
                            if (vm.dataForModerators[i].PublicKey == taskPk) {
                                temp = i;
                                //return;
                            }
                        }
                        if (temp>=0)
                            vm.dataForModerators[temp] = newTask;
                        else
                            vm.dataForModerators.push(newTask);
                    }
                });
            });

            $http
                .post(urlSvc.GetSiteActions)
                .then(function (response) {
                  if (response.data.IsSuccess) {
                      vm.usersForModerators = response.data.Users;
                      vm.dataForModerators = response.data.Tasks.map(function (item) {
                          var newitem = item;
                          newitem.Created = item.Created.replace(/\D+/g, '');
                          newitem.TopicalTo = item.TopicalTo.replace(/\D+/g, '');
                          return newitem;
                      });
                  } else {
                      alert('Server error');
                  }
                }, function (error) {
                  console.log(error.statusText);
              });


            function openMessage(number) {
                var min = vm.dataForModerators[0];
                vm.dataForModerators.forEach(function (item, i, arr) {
                    min = (min.Created < item.Created) ? min : item;
                });


                $http
                    .post(urlSvc.TryToGetTaskOnModeration, { taskPk: min.PublicKey })
                    .then(function (response) {
                          if (response.data.IsSuccess) {
                              if (!response.data.IsBusy) {
                                  vm.sendMessage = min;
                                  vm.settingActive = true;
                              } else {
                                  if (number > vm.dataForModerators.length) {
                                      alert('No free jobs');
                                      return;
                                  }
                                  openMessage(++number);
                              }
                          }
                    }, function () {
                        alert('Server Request Error');
                    });
            };

            vm.numbergroups = function (group) {
                vm.groups.forEach(function (item, i, arr) {
                    if (item.Name === group) {
                        vm.count = i;
                        return;
                    }
                });

            };

            vm.setNotifiAsViewed = function (pk) {
                $resource(urlSvc.SetTaskNotificationAsViewed, { taskPk: pk }, { 'query': { method: 'POST', isArray: false } }).query().$promise.then(function (response) { });
            }
        }
    }
})();
