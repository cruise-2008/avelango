/**=========================================================
 * Module: access-recover.js
 * Demo for login api
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.pages')
        .controller('RemoveFormController', RemoveFormController);

    RemoveFormController.$inject = ['$http','urlSvc'];
    function RemoveFormController($http, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            // bind here all data from the form
            vm.account = {};
            // place the message if something goes wrong
            vm.authMsg = '';

            vm.login = function () {
                vm.authMsg = '';
                
                if (vm.recoverForm.$valid) {

                    $http
                      .post(urlSvc.PasswordRecovery, { email: vm.account.email })
                      .then(function (response) {
                          // assumes if ok, response is an object with some data, if not, a string with error
                          // customize according to your api
                          if (!response.data.Success) {
                              vm.authMsg = 'Incorrect credentials.';
                          }
                          else {
                              window.location.assign(response.data.Path);


                          }
                      }, function () {
                          vm.authMsg = 'Server Request Error';
                      });
                }
                else {
                    // set as dirty if the user click directly to login so we show the validation messages
                    /*jshint -W106*/
                    vm.recoverForm.account_email.$dirty = true;
                   
                }
            };
        }
    }
})();
