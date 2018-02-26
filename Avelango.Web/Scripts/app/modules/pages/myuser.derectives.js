/**=========================================================
 * Module: datatable,js
 * Angular Datatable controller
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.pages')
        .directive('helloWorld', function () {
            return {
                restrict: 'EA',
                //replace: true,
                template: '<div id="carousel-example-generic" class="carousel slide" data-ride="carousel"><div class="carousel-inner"><div class="item active"><img src="/Images/carousel/1.jpg" alt="..."></div><div class="item active"><img src="/Images/carousel/2.jpg" alt="..."></div></div></div>'
                //link: function (scope, elem, attrs) {
                //    elem.bind('click', function () {
                //        //elem.css('background-color', 'black');
                //        scope.$apply(function () {
                //            scope.color = "black";
                //        });
                //    });
                //    elem.bind('mouseover', function () {
                //        elem.css('cursor', 'pointer');
                //    });
                //}
            };
        });

 
})();

