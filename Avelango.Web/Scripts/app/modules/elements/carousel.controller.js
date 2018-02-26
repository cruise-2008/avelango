
(function () {
    'use strict';
    angular
        .module('app.extras')
        .controller('AngularCarouselController', angularCarouselController);

    function angularCarouselController() {
        /*
        var vm = this;
        vm.slides = [];
        vm.carouselIndex = 0;
        vm.colors = ['#fc0003', '#f70008', '#f2000d', '#ed0012', '#e80017', '#e3001c', '#de0021', '#d90026', '#d4002b', '#cf0030', '#c90036', '#c4003b', '#bf0040', '#ba0045', '#b5004a', '#b0004f', '#ab0054', '#a60059', '#a1005e', '#9c0063', '#960069', '#91006e', '#8c0073', '#870078', '#82007d', '#7d0082', '#780087', '#73008c', '#6e0091', '#690096', '#63009c', '#5e00a1', '#5900a6', '#5400ab', '#4f00b0', '#4a00b5', '#4500ba', '#4000bf', '#3b00c4', '#3600c9', '#3000cf', '#2b00d4', '#2600d9', '#2100de', '#1c00e3', '#1700e8', '#1200ed', '#0d00f2', '#0800f7', '#0300fc'];
        */

        activate();

        function activate() {
            /*for (var i = 0; i < 1; i++) {
                vm.slides.push({
                    id: i,
                    label: i,
                    img: 'Images/carousel/' + (Math.floor(Math.random()*(5)) + 1).toString() + '.jpg',
                    color: vm.colors[i],
                    odd: (i % 2 === 0)
                });
            }*/

            var count = 5;//количество картинок

            for (var i = 1; i < count+1; i++)
                $("#slide" + i).fadeOut();

            var ind = Math.floor(Math.random() * (count)) + 1;
            $("#slide" + ind).fadeIn(5000);

            setInterval(function () {
                $("#slide" + ind).fadeOut(5000);

                var ind1 = Math.floor(Math.random() * (count)) + 1;

                while(ind1===ind)
                    ind1 = Math.floor(Math.random() * (count)) + 1;

                ind = ind1;

                $("#slide" + ind).fadeIn(5000);

            }, 10000);
        }
    }
})();
