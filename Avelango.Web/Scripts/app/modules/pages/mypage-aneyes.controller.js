
(function () {
    'use strict';

    angular
        .module('app.pages')
        .controller('MyPageAnothersEyesController', MyPageAnothersEyesController);

    MyPageAnothersEyesController.$inject = ['$http', '$rootScope', '$location', '$scope', '$rootScope'];
    function MyPageAnothersEyesController($http, $location, $scope, $rootScope) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
           
        

        }
    }
})();
