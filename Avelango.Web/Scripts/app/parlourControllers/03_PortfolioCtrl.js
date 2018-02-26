(function () {
    'use strict';

    angular
        .module('app.user.portfolio.controller', [])
        .controller('UserPortfolioCtrl', userPortfolioCtrl);

    userPortfolioCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userPortfolioCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {

        var vm = this;
        vm.activate = false;

        $scope.$on('portfolio', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Портфолио" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();