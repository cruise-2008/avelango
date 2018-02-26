(function() {
    'use strict';

    angular
        .module('app.pagination.service', [])
        .factory('$taskJobService', $taskJobService);

    $taskJobService.$inject = ['$http','urlSvc'];

    //var sc = $scope;

    function $taskJobService($http,urlSvc) {
        var vm = this;
        var getNumberPerPage = function (className) {
            var blockHeight = measureElement(className);
            var bodyHeight = $('body').height();
            var totalHeight = bodyHeight - $('.paginator').height() - $('#header').height() - (bodyHeight * 25 / 100);
            return Math.round((totalHeight / blockHeight));
        };

        var measureElement = function (className) {
            var $el = $('<div class="list-item ' + className + '"></div>');
            $el.appendTo('body');
            var elemHieght = $el.css('height').match(/\d+/)[0] * 1;
            $el.remove();
            return elemHieght;
        }


        vm.coords = {};
        vm.allIds = [];
        vm.allPubKeys = [];
        vm.currentPage = 1;
        vm.numPage = 0;
        vm.numberPerPage = {};


        return {

            getLasrIdNumber: function () {
                return parseInt(vm.allIds[vm.allIds.length - 1].replace(/\D+/g, ''));
            },

            getDataInfo: function (jData, url, callback) { ////getFilteredTask
                $http({
                    url: url,
                    method: 'POST',
                    data: jData
                }).then(callback);
            },

            getReloadPaginationData: function (jobs) {
                vm.allIds = [];
                vm.allPubKeys = [];
                for (var i = 0; i < jobs.length ; i++) {
                    vm.allIds.push(jobs[i].id);
                    vm.allPubKeys.push(jobs[i].PublicKey);
                    vm.numPage = Math.ceil(vm.allIds.length / vm.numberPerPage);
                    var showThis = vm.allIds.slice(0, vm.numberPerPage);
                }
                return { ids: showThis, buttonNumbers: [vm.currentPage, vm.currentPage + 1, vm.currentPage + 2, vm.numPage] };
            },

            setPlace: function (lat, lon) {
                vm.coords.lat = lat;
                vm.coords.lon = lon;
            },

            getPaginationLenght: function (className) {
                vm.numberPerPage = getNumberPerPage(className);
                return vm.numberPerPage * 5;
            },

            getPaginationData: function (data) {
                for (var i = 0; i < data.length; i++) {
                    data[i].id = "id" + i;
                    vm.allIds.push(data[i].id);
                    vm.allPubKeys.push(data[i].PublicKey);
                }
                vm.numPage = Math.ceil(vm.allIds.length / vm.numberPerPage);
                var showThis = vm.allIds.slice(0, vm.numberPerPage);
                return { ids: showThis, buttonNumbers: [vm.currentPage, vm.currentPage + 1, vm.currentPage + 2, vm.numPage] }
            },
            

            stepForward: function () {
                var begin = (vm.currentPage * vm.numberPerPage);
                var end = begin + vm.numberPerPage;
                var btns = [];

                
                var getNewDatas = {}; ////getNewTasks 
                if (vm.currentPage >= vm.numPage - 1) {
                    getNewDatas = { lat: vm.coords.lat, lon: vm.coords.lon, numberPerPage: vm.numberPerPage, allPubKeys: vm.allPubKeys };
                }

                vm.currentPage++;
                btns = [vm.currentPage, vm.currentPage + 1, vm.currentPage + 2, vm.numPage]
                return {
                    ids: vm.allIds.slice(begin, end),
                    buttonNumbers: btns,
                    getNewDatas: getNewDatas
                };
            },

            stepBack: function () {
                if (vm.currentPage > 1) vm.currentPage--;
                var begin = (vm.currentPage * vm.numberPerPage) - vm.numberPerPage;
                var end = (vm.currentPage * vm.numberPerPage);
                return { ids: vm.allIds.slice(begin, end), buttonNumbers: [vm.currentPage, vm.currentPage + 1, vm.currentPage + 2, vm.numPage] };
            },

            stepByCount: function (numberpage) {
                vm.currentPage = numberpage;
                var begin = (vm.currentPage * vm.numberPerPage) - vm.numberPerPage;
                var end = (vm.currentPage * vm.numberPerPage);
                var startGetNewDatas = {}; ////startGetNewTasks

                if (vm.currentPage > vm.numPage - 3) {
                    startGetNewDatas = { lat: vm.coords.lat, lon: vm.coords.lon, numberPerPage: vm.numberPerPage, allPubKeys: vm.allPubKeys };
                }
                
                return {
                    ids: vm.allIds.slice(begin, end),
                    buttonNumbers: [vm.currentPage, vm.currentPage + 1, vm.currentPage + 2, vm.numPage],
                    startGetNewDatas: startGetNewDatas
                };
            }
        }
    }
})();