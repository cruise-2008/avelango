(function () {
    'use strict';

    angular
        .module('app.tables')
        .controller('ModeratorOrdeDataTableController', moderatorOrdeDataTableController);

    moderatorOrdeDataTableController.$inject = ['$resource', 'DTOptionsBuilder', 'DTColumnDefBuilder', 'sweetAlert', '$uibModal', '$scope', '$http', '$rootScope', 'urlSvc'];//,'DatatableUsers'
    function moderatorOrdeDataTableController($resource, DTOptionsBuilder, DTColumnDefBuilder, sweetAlert, $uibModal, $scope, $http, $rootScope, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {

            vm.causeDeact = {};

            $rootScope.$on('newTask', function (e, message) {
                var messag = JSON.parse(message);
                messag.topicalto = messag.topicalto.replace(/\D+/g, '');
                $scope.$apply(function(){
                    $scope.mmm.dataForModerators.push(messag);
                });
            });

            vm.dtOptions = DTOptionsBuilder.newOptions();
            vm.dtColumnDefs = [
                DTColumnDefBuilder.newColumnDef(0),
                DTColumnDefBuilder.newColumnDef(1)
            ];

            vm.deactivate = deactivate;

            vm.send = function () {
                if (vm.myForm.$valid) {
                    $scope.mmm.sendMessage.Created = '/Date(' + $scope.mmm.sendMessage.Created + ')/';
                    $scope.mmm.sendMessage.TopicalTo = '/Date(' + $scope.mmm.sendMessage.TopicalTo + ')/';
                    $scope.mmm.settingActive = false;
                    var jData = JSON.stringify({ task: $scope.mmm.sendMessage, success: true });
                    var req = {
                        url: urlSvc.TaskChecked,
                        method: 'POST',
                        data: jData
                    };
                    $http(req)
                        .then(function(response) {
                            if (response.data.IsSuccess) {
                                sweetAlert.swal('Sended', 'Your message has been sended', 'success');
                            } else {
                                sweetAlert.swal('Server Error', 'Some Error', 'error');
                            }
                        }, function(error) {
                            console.log('Error' + error.statusText);
                        });
                } else {
                    vm.myForm.groups.$dirty = true;
                    vm.myForm.subgroups.$dirty = true;
                    vm.myForm.description.$dirty = true;
                    vm.myForm.title.$dirty = true;
                }
            };

            vm.validationCleaner = function() {
                vm.myForm.groups.$dirty = false;
                vm.myForm.subgroups.$dirty = false;
                vm.myForm.description.$dirty = false;
                vm.myForm.title.$dirty = false;
            }

            function deactivate() {
                $scope.mmm.sendMessage.Created = '/Date('+$scope.mmm.sendMessage.Created+')/';
                $scope.mmm.sendMessage.TopicalTo = '/Date(' + $scope.mmm.sendMessage.TopicalTo + ')/';
                var keys = Object.keys(vm.causeDeact);
                var jData = JSON.stringify({ task: $scope.mmm.sendMessage, success: false, causes: keys });//
                var req = {
                    url: urlSvc.TaskChecked,
                    method: 'POST',
                    data: jData
                };

                $http(req)
                      .then(function (response) {
                        if (response.data.IsSuccess) {
                            $scope.deactwindow = false;
                            $scope.mmm.settingActive = false;
                            sweetAlert.swal('Sended', 'Your message has been sended', 'success');
                        } else {
                            sweetAlert.swal('Try again', 'Some Error', 'error');
                        }

                    }, function () {
                          $scope.deactwindow = false;
                          $scope.mmm.settingActive = false;
                          sweetAlert.swal('Server Error', 'Some Error', 'error');
                      });
            };

        }
    }
})();
