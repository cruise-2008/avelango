(function () {
    'use strict';

    angular
        .module('app.global.parlour.controller', [])
        .controller('GlobalParlourCtrl', GlobalParlourCtrl);

    GlobalParlourCtrl.$inject = ['$http', '$rootScope', '$location', '$scope', 'urlSvc', '$timeout'];
    function GlobalParlourCtrl($http, $rootScope, $location, $scope, urlSvc,  $timeout) {

        var vm = this;

        $("div#leftside").height($("body").height());

        $scope.clickOn = 'StocksAndNews';
        $timeout(function () {
            $rootScope.$broadcast("stocksAndNews");
        });

        $scope.setUnactive = function (event) {
            $('.menu-prof li a').removeClass('active');
            $(event.currentTarget).addClass('active');
        }

        
        $scope.changeContent = function (con) {
            $scope.clickOn = con;

            switch (con) {
                case "StocksAndNews": { $timeout(function () { $rootScope.$broadcast("stocksAndNews"); }); break; }
                case "UserInfo": { $timeout(function () { $rootScope.$broadcast("userInfo"); }); break; }
                case "Portfolio": { $timeout(function () { $rootScope.$broadcast("portfolio"); }); break; }
                case "UserTask": { $timeout(function () { $rootScope.$broadcast("userTask"); }); break; }
                case "UserExecutes": { $timeout(function () { $rootScope.$broadcast("userExecutes"); }); break; }
                case "UserGroup": { $timeout(function () { $rootScope.$broadcast("userGroup"); }); break; }
                case "UserMoney": { $timeout(function () { $rootScope.$broadcast("userMoney"); }); break; }
                case "UserFavorites": { $timeout(function () { $rootScope.$broadcast("userFavorites"); }); break; }
                case "UserReviews": { $timeout(function () { $rootScope.$broadcast("userReviews"); }); break; }
                case "UserExaminations": { $timeout(function () { $rootScope.$broadcast("userExaminations"); }); break; }
                case "UserAwards": { $timeout(function () { $rootScope.$broadcast("userAwards"); }); break; }
            }

        }


        $http({
                url: urlSvc.GetInitialPageData,
                method: 'POST'
            })
            .then(function(result) {

                if (result.data.IsSuccess) {
                    vm.newMessagesCount = result.data.NewMessages;
                    vm.incompleteTasksPositive = result.data.IncompleteTasks.IncompleteTasksPositive;
                    vm.incompleteTasksNegative = result.data.IncompleteTasks.IncompleteTasksNegative;
                    vm.incompleteJobsPositive = result.data.IncompleteJobs.IncompleteJobsPositive;
                    vm.incompleteJobsNegative = result.data.IncompleteJobs.IncompleteJobsNegative;
                }
            });

    }
})();