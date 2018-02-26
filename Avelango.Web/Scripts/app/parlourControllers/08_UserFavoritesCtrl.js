(function () {
    'use strict';

    angular
        .module('app.user.favorites.controller', [])
        .controller('UserFavoritesCtrl', userFavoritesCtrl);

    userFavoritesCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc', 'sweetAlert'];
    function userFavoritesCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {
    
        var vm = this;
        vm.activate = false;

        $scope.$on('userFavorites', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    sweetAlert.swal('Извините, Страница "Избранное" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();