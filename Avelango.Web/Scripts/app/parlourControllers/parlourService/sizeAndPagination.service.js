(function () {
    'use strict';

    angular
        .module('app.sizeAndPagination.service', [])
        .factory('$parlourTicketService', $parlourTicketService);

    $parlourTicketService.$inject = ['$http', 'urlSvc'];

    function $parlourTicketService($http, urlSvc) {

        var vm = this;


        var measureElement = function (className) {

            var $element = $('<tr class="trHeight ' + className + '"></tr>');
            $element.appendTo('body');
            var elemHieght = $element.css('height').match(/\d+/)[0] * 1;
            $element.remove();
            return elemHieght;

        };

        var getNumberPerPage = function (className) {

            var blockHeight = measureElement(className);
            var bodyHeight = $('body').height();
            var totalHeight = bodyHeight - $('.paginator').height() - $(".row innertopblock").height() - $('#header').height() - (bodyHeight * 25 / 100);
            return Math.round((totalHeight / blockHeight));

        };


        return {
            getPaginationLenght: function (className) {
                vm.numberPerPage = getNumberPerPage(className);
                return vm.numberPerPage;
            }
        }
    }
})();