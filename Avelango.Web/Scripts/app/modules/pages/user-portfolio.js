(function () {
    'use strict';

    angular
        .module('app.pages',[])
        .controller('UserPortfolioCtrl', UserPortfolioCtrl);

    UserPortfolioCtrl.$inject = ['$http', '$rootScope', '$location', '$scope', '$resource', 'urlSvc'];
    function UserPortfolioCtrl($http, $rootScope, $location, $scope, $resource, urlSvc) {
        var vm = this;

        activate();

        ////////////////

        function activate() {
            vm.portfolio = {};
            vm.setImage = function (imageUrl) {
                vm.mainImageUrl = imageUrl;
            };
            var url;
            vm.sendchenges = function (option) {
                switch (option) {
                    case 'userinfo':
                        url = '';
                        break;
                    case '':
                        url = '';
                        break;
                    case 5:
                        //alert('Перебор');
                        break;
                    default:
                        url = urlSvc.GetPortfolioData;
                }
                $http
                      .post(url)
                      .then(function (response) {
                          vm.portfolio = response.data;
                      }, function (error) {
                          vm.authMsg = 'Server Request Error';
                          console.log('Error' + error);
                      });

            };
            vm.sendchenges();
        }
    }
})();