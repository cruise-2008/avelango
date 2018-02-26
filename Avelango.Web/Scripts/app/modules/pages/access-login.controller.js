(function () {
    'use strict';
    angular.module('app.pages').controller('loginFormController', loginFormController);
    loginFormController.$inject = ['$http', '$rootScope', '$scope', '$uibModal', 'urlSvc'];

    function loginFormController($http, $rootScope, $scope, $uibModal,urlSvc) {
        var vm = this;
        activate();

        function activate() {
            vm.account = {};
            vm.authMsg = "";

            modalInstanceCtrl.$inject = ['$scope', '$uibModalInstance', '$rootScope'];
            function modalInstanceCtrl($scope, $uibModalInstance, $rootScope) {
                $scope.ok = function () { $uibModalInstance.close('closed'); };
                $scope.cancel = function () { $uibModalInstance.dismiss('cancel'); };
                $rootScope.$on("closeModal", function () { $uibModalInstance.close('closed'); });
            }

            vm.socialmodalopen = function (template) {http://localhost:3300/../../../../Hubs
                var modalInstance = $uibModal.open({
                    template: template,
                    controller: modalInstanceCtrl
                    //size: size
                });
                var state = $('#modal-state');
                modalInstance.result.then(function () {
                    state.text('Modal dismissed with OK status');
                }, function () {
                    state.text('Modal dismissed with Cancel status');
                });
            }

            vm.slogin = function(data) {
                $http({ url: '/Account/Slogin', method: 'POST', data: { social: data } })
                    .then(function (result) {
                        if (result.data.IsSuccess) {
                            vm.socialmodalopen(result.data.Html);
                        }
                    },
                    function () {
                        vm.authMsg = 'Server Request Error';
                    });
            }

            vm.login = function () {
                vm.authMsg = '';
                if (vm.loginForm.$valid) {
                    vm.count = true;
                    $http
                      .post(urlSvc.Login, { UserName: vm.account.email, Password: vm.account.password, RememberMe: vm.account.remember })
                      .then(function (response) {
                          if (!response.data.IsEnabled) {
                              vm.count = false;
                              vm.authMsg = 'Incorrect credentials.';
                          } else if (response.data.RedirectPath) {
                              $rootScope.ParlourPath = response.data.Path;
                              $rootScope.IsAuthanticated = response.data.IsEnabled;
                              $rootScope.ImagePath = response.data.ImagePath;
                              window.location = response.data.RedirectPath + "?pk=" + response.data.PublicKey;
                          } else {
                              $rootScope.ParlourPath = response.data.Path;
                              $rootScope.IsAuthanticated = response.data.IsEnabled;
                              $rootScope.ImagePath = response.data.ImagePath;
                              window.location = response.data.Path;
                          }
                      }, function () {
                          vm.authMsg = 'Server Request Error';
                          vm.count = false;
                      });
                }
                else {
                    vm.loginForm.account_email.$dirty = true;
                    vm.loginForm.account_password.$dirty = true;
                    vm.count = false;
                }
            };
        }
    }
})();
