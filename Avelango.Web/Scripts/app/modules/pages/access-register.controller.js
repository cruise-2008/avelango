/**=========================================================
 * Module: access-register.js
 * Demo for register account api
 =========================================================*/

(function() {
    'use strict';

    angular
        .module('app.pages')
        .controller('RegisterFormController', RegisterFormController);

    RegisterFormController.$inject = ['$http', '$state', 'urlSvc'];
    function RegisterFormController($http, $state, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
          // bind here all data from the form
          vm.account = {};
          // place the message if something goes wrong
         
            
          vm.register = function() {
            vm.authMsg = '';

            if(vm.registerForm.$valid) {

              $http
                .post(urlSvc.Register, { UserName: vm.account.email, Password: vm.register.password, Phone: vm.account.phone })
                .then(function(response) {
                  // assumes if ok, response is an object with some data, if not, a string with error
                  // customize according to your api
                  if ( !response.data.Success ) {
                      vm.authMsg = 'User exist';
                      
                  }else{
                      window.location.assign(response.data.Path);
                  }
                }, function() {
                    vm.authMsg = 'Server error';
                });
            }
            else {
              // set as dirty if the user click directly to login so we show the validation messages
              /*jshint -W106*/
              vm.registerForm.account_email.$dirty = true;
              vm.registerForm.account_phone.$dirty = true;
              vm.registerForm.account_password.$dirty = true;
              vm.registerForm.account_agreed.$dirty = true;
              
            }
          };
        }
    }
})();
