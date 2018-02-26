(function () {
    'use strict';

    angular
        .module('app.validator.service', [])
        .factory('$validator', $validator);

    $validator.inject = [];

    function $validator() {
        return {
            email: function (email) {
                var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
                return regex.test(email);
            },
            name: function (name) {
                var regex = /(\D{3,})|(\D{3,}\w+)$/;
                return regex.test(name);
            },
            smsCode: function (smsCode) {
                var regex = /^\d{8}$/;
                return regex.test(smsCode);
            },
            phoneCode: function (phoneCode) {
                var regex = /\+(\d{1,3})$/;
                return regex.test(phoneCode);
            },
            phoneNumber: function (phoneNumber) {
                var regex = /^[(]{0,1}[0-9]{3}[)\.\- ]{0,1}[0-9]{3}[\.\- ]{0,1}[0-9]{4}$/;
                return regex.test(phoneNumber);
            },
        }
    }
})();