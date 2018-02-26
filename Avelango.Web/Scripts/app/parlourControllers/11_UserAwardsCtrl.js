(function () {
    'use strict';

    angular
        .module('app.user.awards.controller', [])
        .controller('UserAwardsCtrl', userAwardsCtrl);

    userAwardsCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc','sweetAlert'];
    function userAwardsCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {
        
        var vm = this;
        vm.activate = false;

        $scope.$on('userAwards', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Награды" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();