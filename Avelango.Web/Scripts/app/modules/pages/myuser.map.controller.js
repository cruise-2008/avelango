(function () {
    'use strict';

    angular
        .module('app.pages')
        .controller('UserCabinetMap', UserCabinetMap);

    UserCabinetMap.$inject = ['$http', '$rootScope', '$location', '$scope', '$resource', 'urlSvc'];
    function UserCabinetMap($http, $rootScope, $location, $scope, $resource, urlSvc) {

        var vm = this;
       // var pointTab = parlourContrainer.dataset.tab;

        activate();
        getInitialPageData();
        //if (!pointTab) getCommonInitialPageData();


        function activate() {
            vm.userpatches = {};

            vm.changeContent = function(point) {
                vm.userpatches.link = point;
                vm.userpatches.path = 'userallpage';
            }

            $rootScope.$on('$locationChangeSuccess', function () {
                vm.loadpoint = true;
            });
        }

        function getInitialPageData() {
            $http({ url: urlSvc.GetInitialPageData, method: 'POST' })
                .then(function (result) {
                    if (result.data.IsSuccess) {
                        vm.newMessagesCount = result.data.NewMessages;
                        vm.incompleteTasksPositive = result.data.IncompleteTasks.IncompleteTasksPositive;
                        vm.incompleteTasksNegative = result.data.IncompleteTasks.IncompleteTasksNegative;
                        vm.incompleteJobsPositive = result.data.IncompleteJobs.IncompleteJobsPositive;
                        vm.incompleteJobsNegative = result.data.IncompleteJobs.IncompleteJobsNegative;
                    }
                },
                function () {
                    vm.authMsg = 'Server Request Error';
                });
        }

        function getCommonInitialPageData() {
            $http({ url: urlSvc.GetCommonInitialPageData, method: 'POST' })
                .then(function (result) {
                    if (result.data.IsSuccess) {
                        for (var i = 0; i < result.data.InitialPageData.News.length; i++) {
                            vm.news += '<span>' + textCorrect(result.data.InitialPageData.News[i].Html) + '</span>';
                        }
                    }
                },
                function () {
                    vm.authMsg = 'Server Request Error';
                });
        }

        function textCorrect(text) {
            text = text.replace(/\\t/g, '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;');
            text = text.replace(/\\n\\r/g, '<br/>');
            return text;
        }

      ////$resource(urlSvc.GetMyInfo, {}, { 'query': { method: 'GET', isArray: false } }).query().$promise.then(function (persons) {
      ////    var res = persons.Birthday.split("-");
      ////    res[0] = res[0]*1;
      ////    res[1] = res[1]*1;
      ////    res[2] = res[2]*1;
      ////    persons.Birthday = res;
      ////    vm.personalData = persons;
      //// });
    }
})();