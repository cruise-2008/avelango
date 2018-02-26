//(function (){
//    'use strict';

//    angular
//        .module('app.menu.filter.service', [])
//        .factory('$filterMenu', $filterMenu);
        
//    $filterMenu.$inject = ['$http'];
//    function $filtermenu($http) {
//        return {
//            showMenuFilter: function ($scope, id) {
//                $http({
//                    method: 'GET',
//                    url: '/I18N/GetGroupContent',
//                }).success(function (response) {
//                    return response;
//                }).error(function () {
//                    alert("error");
//                    return null;
//                });
//            }
//        }
//    }
//})();