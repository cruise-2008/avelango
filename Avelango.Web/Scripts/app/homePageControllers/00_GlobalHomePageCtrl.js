(function () {
    "use strict";

    angular
        .module("app.global.home.page.controller", [])
        .controller("GlobalHomePageCtrl", globalHomePageCtrl);


    globalHomePageCtrl.$inject = ['$scrollHomePage', '$modalWindowService', '$map', '$http', '$location', '$scope', '$rootScope', 'urlSvc', '$window', '$q', '$timeout', 'sweetAlert'];
    function globalHomePageCtrl($scrollHomePage, $modalWindowService, $map, $http, $location, $scope, $rootScope, urlSvc, $window, $q, $timeout, sweetAlert) {


        var vm = this;
        var currentPosition = 1;
       // $map.getAutoUserLocationPath();


        $scope.changeLayout = function() {
            if (currentPosition !== 1) {
                document.getElementById("main").classList.remove("darktheme");
            } else {
                document.getElementById("main").classList.add("darktheme");
            }
        };

        var setPoinerActive = function () {
            $(".onepage-pagination li a").removeClass("active");
            $($(".onepage-pagination li a")[currentPosition]).addClass("active");
        };

        $scope.newNameClass = function ($event) {
            $(".onepage-pagination li a").removeClass("active");
            $($event.currentTarget).addClass("active"); // Гавно КОД!!
        };

        vm.myIndex = document.getElementsByClassName("sectionscroll");


        $(window).load(function () {
            $("div.sectionscroll").height($("body").height());
            $location.hash("");
            $scrollHomePage.scrollTo(vm.myIndex[1]);
        });

        vm.scrollTo = debounce(function (index) {
            if (vm.e != null) {
                vm.e.preventDefault();
                vm.e.stopPropagation();
            }
            if (currentPosition === 0) {
                currentPosition = 0
            }
            currentPosition = index;
            $location.hash("");
            $scrollHomePage.scrollTo(vm.myIndex[currentPosition]);
            setPoinerActive();
            $scope.changeLayout();

        }, 200);

        var timeout;

        function debounce(func, wait) {
            timeout = null;
            return function () {
                var context = this, args = arguments;
                clearTimeout(timeout);
                timeout = setTimeout(function () {
                    timeout = null;
                    func.apply(context, args);
                }, wait);
            };
        }

        vm.e = null;

        angular.element(document).on("mousewheel DOMMouseScroll", debounce(function (e) {
            vm.e = e;
            e.preventDefault();
            e.stopPropagation();
            

            var isUp = e.originalEvent.detail < 0 || e.originalEvent.wheelDelta > 0 ? true : false;

            if (isUp) {
                if (currentPosition > 0) {
                    currentPosition--;
                    setPoinerActive();
                    $scrollHomePage.scrollTo(vm.myIndex[currentPosition]);
                }
            }
            else {
                if (currentPosition < 3) {
                    currentPosition++;
                    setPoinerActive();
                    $scrollHomePage.scrollTo(vm.myIndex[currentPosition]);
                }
            }
            $scope.changeLayout();
        }, 200));

        
           /// $map.getAutoUserLocationPath();
           /// $map.autocomplite('wizardsearch');
           



        var templUrl = "/groupModalContent.html";
        var controller = globalHomePageCtrl;
        var url = "/Home/GroupsModal";
        var id = "content1";

        $scope.preObj = [];

        angular.element(document).ready(function () {

            $scope.OpenSubgroups = function (group) {
                // $modalWindowService.preloader(group);
                $modalWindowService.openModal(url, id, templUrl, controller);
                //$scope.preObj = group;
                $rootScope.subgroups = group.SubGroups;
                $rootScope.groupICO = group.Ico;
                $rootScope.groupName = group.Text;
            };
            $scope.closeModal = function () {
                $modalWindowService.closeModal(templUrl, controller, currentPosition);
                currentPosition = 0;
            };

        });




        var anchorId = [];

        $http({
            method: "POST",
            url: "/I18N/GetGroupContent",
            data: JSON.stringify({ lang: "ru" })
        }).success(function (response) {

            var a = 0;
            for (var i = 0; i < response.length; i++) {
                a++;
                response[i].Id = "i" + a;
                anchorId.push(response[i].Id);
                for (var j = 0; j < response[i].SubGroups.length; j++) {
                    a++;
                    response[i].SubGroups[j].Id = "i" + a;
                };
            };

            vm.filtergroups = response;
        }).error(function () {
            sweetAlert.swal("Error.");
        });


        //*START PLAY VIDEO*//
        var yt_frame = document.querySelector(".video__player"),
		video_placeholder = document.querySelector(".video__placeholder");

        video_placeholder.addEventListener("click", play);

        function play() {
            yt_frame.contentWindow.postMessage(JSON.stringify({
                'event': "command",
                'func': "playVideo",
                'args': []
            }), "*");
            video_placeholder.style.display = "none";
        }
        //*END PLAY VIDEO*//



    }
})();