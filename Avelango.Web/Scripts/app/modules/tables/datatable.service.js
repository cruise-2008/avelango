(function () {
    'use strict';

    angular
        .module('app.tables')
        .factory('DatatableUsers', DatatableUsers);

    DatatableUsers.$inject = ['$resource', '$scope'];
    function DatatableUsers($resource, $scope) {

        var service = {
            init: init
        };

        return service;

        ////////////////

        function init() {
            return $resource(url, {}, { 'query': { method: 'POST', isArray: true } }).query().$promise.then(function (persons) {
                service.heroes = persons;
            });
        }


    }
})();