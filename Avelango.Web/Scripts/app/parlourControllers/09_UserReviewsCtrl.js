(function () {
    'use strict';

    angular
        .module('app.user.reviews.controller', [])
        .controller('UserReviewsCtrl', UserReviewsCtrl);

    UserReviewsCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc','sweetAlert'];
    function UserReviewsCtrl($http, $rootScope, $scope, urlSvc, sweetAlert) {
    
        var vm = this;
        vm.activate = false;

        $scope.$on('userReviews', function () {
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