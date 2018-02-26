(function () {
    'use strict';

    angular
        .module('app.cookie.service', [])
        .factory('$cookie', $cookie);

    $cookie.inject = ['$cookies', '$rootScope'];

    function $cookie($cookies, $rootScope) {
        var key = "user";
        return {
            getCookie: function () {
                var data = $cookies.getObject(key);
                if (data != undefined) {
                    var user = JSON.parse(data);
                    $rootScope.setUpUser(user);
                }
            },
            setCookie: function () {
                var juser = JSON.stringify($rootScope.user);
                $cookies.putObject(key, juser);
            }
        }
    }
})();