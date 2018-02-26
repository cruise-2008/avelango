(function () {
    'use strict';

    angular
        .module('app.user.executes.controller', [])
        .controller('UserExecutesCtrl', userExecutesCtrl);

    userExecutesCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userExecutesCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {

        var vm = this;
        vm.activate = false;

        $scope.$on('userExecutes', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Мои Работы" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();