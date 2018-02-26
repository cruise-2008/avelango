(function () {
    'use strict';

    angular
        .module('app.scroll.home.page', [])
        .factory('$scrollHomePage', $scrollHomePage);

    $scrollHomePage.inject = [];
    
    function $scrollHomePage() {
        
        return {
            scrollTo: function (sectionscroll) {
                var startY = currentYPosition();
                var stopY = elmYPosition(sectionscroll);
                var distance = stopY > startY ? stopY - startY : startY - stopY;
                if (distance < 100) {
                    scrollTo(0, stopY);
                    return;
                }
                var speed = Math.round(distance / 150);
                if (speed >= 250) speed = 250;
                var step = Math.round(distance / 25);
                var leapY = stopY > startY ? startY + step : startY - step;
                var timer = 0;
                if (stopY > startY) {
                    for (var i = startY; i < stopY; i += step) {
                        setTimeout("window.scrollTo(30, " + leapY + ")", timer * speed);
                        leapY += step;
                        if (leapY > stopY) leapY = stopY;
                        timer++;
                    }
                    return;
                }
                for (var i = startY; i > stopY; i -= step) {
                    setTimeout("window.scrollTo(30, " + leapY + ")", timer * speed);
                    leapY -= step;
                    if (leapY < stopY) leapY = stopY;
                    timer++;
                }

                function currentYPosition() {
                    if ('scrollRestoration' in history) {
                        history.scrollRestoration = 'manual';
                    }
                    if (document.documentElement && document.documentElement.scrollTop)
                        return document.documentElement.scrollTop;
                    if (document.body.scrollTop) return document.body.scrollTop;
                    return 0;
                }

                function elmYPosition(elm) {
                    // var elm = document.getElementById(eID);
                    //$(elm).addClass('move');
                    var y = elm.offsetTop;
                    var node = elm;
                    while (node.offsetParent && node.offsetParent != document.body) {
                        node = node.offsetParent;
                        y += node.offsetTop;
                    }
                    return y;
                }
            }

        };
    }
})();