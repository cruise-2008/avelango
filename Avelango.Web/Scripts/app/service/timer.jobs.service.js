(function () {
    'use strict';


    angular
        .module('app.timer.service', [])
        .factory('$timer', $timer);


    $timer.inject = ['$interval'];
    var padLeft;

    function $timer($interval) {
        return {
            getTopicalTo: function (date) {
                var cdate = date.match(/\d+/g)[0];
                var dateNow = new Date();
                var dateTopical = new Date(parseInt(cdate));
                var diffMs = (dateTopical - dateNow); 
                var diffDays = Math.floor(diffMs / 86400000); 
                var diffHrs = Math.floor((diffMs % 86400000) / 3600000); 
                var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); 
                return { d: diffDays < 0 ? 0 : diffDays, h: diffHrs < 0 ? 0 : diffHrs, m: diffMins < 0 ? 0 : diffMins };
            },
            getTimetoparlour: function(date) {
                var dateTopical = new Date(parseInt(date));
                var dateDay = padLeft(dateTopical.getDate());
                var dateMonth = padLeft(dateTopical.getMonth() + 1);
                var getYear = dateTopical.getFullYear();
                return { d: dateDay + "/" + dateMonth + "/" + getYear };
            }
        }
    }

    padLeft = function (date) {
        var str = date.toString();
        return "00".substring(0, "00".length - str.length) + str;
    };
})();