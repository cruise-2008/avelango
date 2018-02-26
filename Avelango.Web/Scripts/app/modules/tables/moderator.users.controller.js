(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('ModeratorUsersDataTableController', moderatorUsersDataTableController);

    moderatorUsersDataTableController.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', '$http', '$rootScope', 'urlSvc'];//,'DatatableUsers'
    function moderatorUsersDataTableController($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, $http, $rootScope,urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            vm.causeDeact = {};
            vm.sendUser = {};
            //$rootScope.$on('newTask', function (e, message) {
            //    var messag = JSON.parse(message);
            //    messag.topicalto = messag.topicalto.replace(/\D+/g, '');
            //    $scope.$apply(function () {
            //        $scope.mmm.dataForModerators.push(messag);
            //    });

            //});

            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1)

            ];

            vm.deactivate = deactivate;
            vm.senduser = senduser;
            vm.openUser = openUser;

            function openUser(number) {
                var min = $scope.mmm.usersForModerators[0];
                $http
                      .post(urlSvc.TryToGetUserOnModeration, { userPk: min.Pk })
                      .then(function (response) {
                          if (response.data.IsSuccess) {
                              if (!response.data.IsBusy) {
                                  min.AccountCreated = min.AccountCreated.replace(/\D+/g, '');
                                  vm.sendUser = min;
                                  vm.settingActive = true;
                              } else {
                                  if (number > $scope.mmm.usersForModerators.length) {
                                      sweetAlert.swal('No free jobs', 'Try later', 'error');
                                      return;
                                  }
                                  openUser(++number);
                              }
                          }
                      }, function () {
                          alert('Server Request Error');
                      });
            };

            function senduser () {
                //$scope.mmm.sendMessage.Created = '/Date(' + $scope.mmm.sendMessage.Created + ')/';
                //$scope.mmm.sendMessage.TopicalTo = '/Date(' + $scope.mmm.sendMessage.TopicalTo + ')/';
                vm.settingActive = false;
                var jData = JSON.stringify({ user: vm.sendUser, success: true });
                var req = {
                    url: urlSvc.UserChecked,
                    method: 'POST',
                    data: jData
                    //{
                    //task:JSON.stringify($scope.mmm.sendMessage),
                    //success: true
                    //}
                };
                $http(req)
                    .then(function (response) {
                        if (response.data.IsSuccess) {
                            sweetAlert.swal('Sended', 'Your message has been sended', 'success');
                        } else {
                            sweetAlert.swal('Server Error', 'Some Error', 'error');
                        }
                    }, function (error) {
                        console.log('Error' + error.statusText);
                    });
            };

            function deactivate() {
                var keys = Object.keys(vm.causeDeact);
                var jData = JSON.stringify({ user: vm.sendUser, success: false, causes: keys });//
                var req = {
                    url: urlSvc.UserChecked,
                    method: 'POST',
                    data: jData
                };
                $http(req)
                      .then(function (response) {
                          if (response.data.IsSuccess) {
                              $scope.mmm.deactwindow = false;
                              vm.settingActive = false;
                              sweetAlert.swal('Sended', 'Your message has been sended', 'success');
                          } else {
                              sweetAlert.swal('Try again', 'Some Error', 'error');
                          }
                      }, function () {
                          $scope.mmm.deactwindow = false;
                          vm.settingActive = false;
                          sweetAlert.swal('Server Error', 'Some Error', 'error');
                      });
            };

        }
    }
})();
