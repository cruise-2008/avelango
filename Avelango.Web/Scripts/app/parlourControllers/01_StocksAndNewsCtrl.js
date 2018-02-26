(function () {
    'use strict';

    angular
        .module('app.stocks.and.news.controller', ['ui.bootstrap'])
        .controller('StocksAndNewsCtrl', StocksAndNewsCtrl);


    StocksAndNewsCtrl.$inject = ['$http', '$rootScope', '$scope', 'urlSvc'];
    function StocksAndNewsCtrl($http, $rootScope, $scope, urlSvc) {

        var vm = this;
        vm.activate = false;





        $scope.$on('stocksAndNews', function () {
            if (vm.activate === true) {
                return;
            } else {
                activate();
                function activate() {
                    $scope.myInterval = 1000;
                    $scope.slides = [
                        {
                            image: '/Images/imgForParlourCarusel/IMG_5686.JPG',
                            id: "testImg"
                            },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_7019.JPG',
                            id: "testImg"
                            },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_1980_1.JPG',
                            id: "testImg"
                            },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_2429.JPG',
                            id: "testImg"
                        },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_2834.JPG',
                            id: "testImg"
                        },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_5345.JPG',
                            id: "testImg"
                        },
                        {
                            image: '/Images/imgForParlourCarusel/IMG_1980_1.JPG',
                            id: "testImg"
                        }
                    ];

                    // alert('Извините, Страница "Акции и Новости" в Разработке!');
                    vm.activate = true;
                }
            }
        });
    }
})();






