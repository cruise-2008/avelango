(function (){
    'use strict';

    angular
        .module('app.open.tickets',[])
        .factory('$OpenTicketsService', $OpenTicketsService);

    $OpenTicketsService.inject = ['$http'];


    function $OpenTicketsService($http) {

        return {

            showTaskContent: function ($scope, id, callback) {
                $http({
                    method: 'GET',
                    url: '/Task/TaskCard',
                }).success(function (response) {
                    $("#" + id).append($compile(response.data)($scope));
                }).error(function () {
                    alert("error");
                    return null;
                });
            },

            showExecutorsContent: function ($scope, id, callback) {
                $http({
                    method: 'GET', 
                    url: '/User/ExecutorsCard',
                }).success(function (response) {
                    $("#" + id).append($compile(response.data)($scope));
                }).error(function () {
                    alert("error");
                    return null;
                });
            },
        }
    }
});