(function() {
    'use strict';

    angular.module('app.elements')
        .service('starsSvc', starsSvc);
    starsSvc.$inject = [];

    function starsSvc() {
        var ratingToMass = function(raiting) {
            switch (raiting) {
                case 0:
                    return ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                case 0.5:
                    return ['fa-star-half-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                case 1:
                    return ['fa-star', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                case 1.5:
                    return ['fa-star', 'fa-star-half-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                case 2:
                    return ['fa-star', 'fa-star', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
                case 2.5:
                    return ['fa-star', 'fa-star', 'fa-star-half-o', 'fa-star-o', 'fa-star-o'];
                case 3:
                    return ['fa-star', 'fa-star', 'fa-star', 'fa-star-o', 'fa-star-o'];
                case 3.5:
                    return ['fa-star', 'fa-star', 'fa-star', 'fa-star-half-o', 'fa-star-o'];
                case 4:
                    return ['fa-star', 'fa-star', 'fa-star', 'fa-star-o', 'fa-star-o'];
                case 4.5:
                    return ['fa-star', 'fa-star', 'fa-star', 'fa-star', 'fa-star-half-o'];
                case 5:
                    return ['fa-star', 'fa-star', 'fa-star', 'fa-star', 'fa-star'];
                default:
                    return ['fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o', 'fa-star-o'];
            };
        }

        return {
            ratingToMass: ratingToMass
        }
    }
})();