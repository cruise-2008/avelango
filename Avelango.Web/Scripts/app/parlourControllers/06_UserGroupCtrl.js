(function () {
    'use strict';

    angular
        .module('app.user.group.controller', [])
        .controller('UserGroupCtrl', userGroupCtrl);

    userGroupCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userGroupCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {

        var vm = this;
        vm.activate = false;

        $scope.$on('userGroup', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Группы и подписки" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();