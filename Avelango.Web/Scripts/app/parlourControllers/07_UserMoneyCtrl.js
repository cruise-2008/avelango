(function () {
    'use strict';

    angular
        .module('app.user.money.controller', [])
        .controller('UserMoneyCtrl', userMoneyCtrl);

    userMoneyCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userMoneyCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {
    
        var vm = this;
        vm.activate = false;

        $scope.$on('userMoney', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Мои Деньги" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();