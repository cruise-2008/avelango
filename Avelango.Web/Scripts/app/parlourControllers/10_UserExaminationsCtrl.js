(function () {
    'use strict';

    angular
        .module('app.user.examination.controller', [])
        .controller('UserExaminationsCtrl', userExaminationsCtrl);

    userExaminationsCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userExaminationsCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {
    
        var vm = this;
        vm.activate = false;

        $scope.$on('userExaminations', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Экзамены" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();