(function () {
    'use strict';

    angular
        .module('app.login.service', [])
        .factory('$login', $login);

    $login.inject = ['$http'];

    function $login($http) {
        return {
            showWizard: function ($scope, id) {
                $http({
                    method: 'GET',
                    url: '/Wizard/LoginWizard'
                }).success(function (response) {
                    $("#" + id).append($compile(response.data)($scope));
                }).error(function () {
                    return null;
                });
            },
            checkPhone: function (phone, callback) {
                $http({
                    method: 'POST',
                    data: JSON.stringify({ phone: phone }),
                    url: '/Account/CheckPhone'
                }).then(function (result) {
                    callback(result.data);
                });
            },
            registration: function (smsCode, name, callback) {
                $http({
                    method: 'POST',
                    data: JSON.stringify({ code: smsCode, name: name }),
                    url: '/Account/Register'
                }).then(function (result) {
                    callback(result.data);
                });
            },
            checkPass: function (phone, password, callback) {
                $http({
                    method: 'POST',
                    data: JSON.stringify({ phone: phone, password: password }),
                    url: '/Account/Login'
                }).then(function (result) {
                    callback(result.data);
                });
            }
        }
    }
})();