(function() {
    'use strict';

    angular
        .module('app.custom')
        .controller('MainController', mainController);

    mainController.$inject = ['$location', '$http', '$scope', '$rootScope', 'Userauth', 'signalRSvc', '$anchorScroll', '$translate', '$window', 'urlSvc'];
    function mainController($location, $http, $scope, $rootScope, Userauth, signalRSvc, $anchorScroll, $translate, $window,urlSvc) {
        // for controllerAs syntax
        var vm = this;

        activate();

        function activate() {
            vm.asideToggled = false;

            vm.login = function(pn, pk) {
                this.showme = 'ball-clip-rotate';
                var req = {
                    url: urlSvc.SetCallBack,
                    method: 'GET',
                    params: { pageName: pn, publicKey: pk == undefined ? 'null' : pk }
                };
                $http(req).then();
                $window.location.href = '/Account/Login';
            }

            $scope.getClass = function (path) {
                return ($location.path().substr(0, path.length) === path) ? 'active' : '';
            }

            $scope.$on('$locationChangeSuccess', function (event, url, oldUrl, state, oldState) {
                $scope.currentPage = $location.path();
                vm.showme = null;
                vm.showmecabinet = null;
                $translate.refresh();
            });
           
            var updateImg = function (avapath) {
                vm.ImagePath = avapath[1];
            }
            $rootScope.$on("newImgPath", function (event, avapath) {
                updateImg(avapath);
            });
          
            //var updateGreetingMessage = function (text) {
            //    $scope.text = text;
            //}
            
            //$rootScope.$on("acceptGreet", function (e, message) {
            //    $scope.$apply(function () {
            //        updateGreetingMessage(message);
            //    });
            //});
            Userauth.init().then(function () {
                if (Userauth.IsAuthanticated) {
                    vm.ParlourPath = Userauth.ParlourPath;
                    vm.IsAuthanticated = Userauth.IsAuthanticated;
                    vm.ImagePath = Userauth.ImagePath;
                    //vm.lab = Userauth.arr[0];
                    vm.SessionId = Userauth.SessionId;
                    vm.Groups = Userauth.Groups;
                    signalRSvc.initialize(vm.SessionId,vm.Groups);
                }

            });

            vm.logoff = function () {
                $http
               .get('/Account/LogOff')
               .then(function () {
                   // assumes if ok, response is an object with some data, if not, a string with error
                   // customize according to your api
                       window.location = '/';
                       //$location.path(response.data.Path);
               });
            };

            vm.gotoUp = function() {  
                $anchorScroll();
            };
        }
    }
})();
