/**=========================================================
 * Module: demo-pagination.js
 * Provides a simple demo for pagination
 =========================================================*/
(function() {
    'use strict';

    angular
        .module('app.bootstrapui')
        .controller('PaginationDemoCtrl', PaginationDemoCtrl);

    PaginationDemoCtrl.$inject = ['$scope'];


    function PaginationDemoCtrl($scope) {
        
        var sc = $scope;

        sc.filteredTasks = [],
        sc.currentPage = 1,
        sc.numberPerPage = 5,
        sc.maxsize = 3;
        

        sc.makeTodos = function () {
            sc.todos = [];
            for (i = 0; i < data.Jobs.length; i++) {
                sc.todos.push(data.Jobs[i]);
                if (i > sc.numberPerPage) break;
            }
        }();

        sc.numPage = function () {
            return Math.ceil(sc.todos.length / sc.numPerPage);
        };

        sc.$watch('currentPage + numPerPage', function (){
            var begin = ((sc.currentPage - 1) * sc.numPerPage),
                end = begin + sc.numPerPage;

            sc.filteredTasks = sc.todos.slice(begin, end);
        });
        
    }
})();




//function activate() {
//vm.totalItems = 64;
//vm.currentPage = 4;
//
//vm.setPage = function (pageNo) {
//vm.currentPage = pageNo;
//  };
//
//vm.pageChanged = function() {
//    console.log('Page changed to: ' + vm.currentPage);
//  };
//
//vm.maxSize = 5;
//vm.bigTotalItems = 175;
//vm.bigCurrentPage = 1;
//}