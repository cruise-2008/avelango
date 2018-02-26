/**=========================================================
 * Module: sidebar-menu.js
 * Handle sidebar collapsible elements
 =========================================================*/

(function () {
    'use strict';

    angular
        .module('app.sidebar')
        .controller('SidebarController', SidebarController);

    SidebarController.$inject = ['SidebarLoader', '$rootScope', '$scope', '$state', '$http', 'urlSvc', '$translate'];
    function SidebarController(SidebarLoader, $rootScope, $scope, $state, $http, urlSvc, $translate) {
        

        var vm = this;
        var sc = $scope;
        activate();

        //vm.trgroups = {};

        ////////////////

        function activate() {
            var collapseList = [];
            var anchorId = [];

            $http({
                method: 'POST',
                url: '/I18N/GetGroupContent',
                data: JSON.stringify({ lang: 'ru' })
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
                alert("error");
            });

            
            $scope.accordion = {
                current: null
            };


            sc.closeSideBarWindow = true;

            (function () {

                var position = 0;

                var getFilterBlock = function () {
                    if (vm.filters == undefined || vm.filters.length == 0) {
                        vm.filters = $(document.getElementsByClassName('filterlist')[0]);
                    }
                }

                vm.down = function () {
                    if (position < anchorId.length) position++;
                    getFilterBlock();
                    vm.filters.animate({ scrollTop: $($('#' + anchorId[position])[0]).offset().top }, "slow");
                }

                vm.up = function () {
                    if (position > 0) position--;
                    getFilterBlock();
                    vm.filters.animate({ scrollTop: $($('#' + anchorId[position])[0]).offset().top }, "slow");

                }
            })();
            $rootScope.closeCurentSideBar = function () {
                sc.closeSideBarWindow = false;
                $(".content-holder").animate({
                    'marginLeft': '7%'
                }, 500);
            }
            $rootScope.openCurentSideBar = function () {
                sc.closeSideBarWindow = true;
                $(".content-holder").animate({
                    'marginLeft': '25%'
                }, 100);
            }




            // demo: when switch from collapse to hover, close all items
            //$rootScope.$watch('app.layout.asideHover', function(oldVal, newVal){
            //  if ( newVal === false && oldVal === true) {
            //    closeAllBut(-1);
            //  }
            //});


            // Load menu from json file
            // ----------------------------------- 

            SidebarLoader.getMenu(sidebarReady);

            function sidebarReady(items) {
                $scope.menuItems = items;
            }

            // Handle sidebar and collapse items
            // ----------------------------------

            $scope.getMenuItemPropClasses = function (item) {
                return (item.heading ? 'nav-heading' : '') +
                       (isActive(item) ? ' active' : '');
            };

            $scope.addCollapse = function ($index, item) {
                collapseList[$index] = !isActive(item);
            };

            $scope.isCollapse = function ($index) {
                return (collapseList[$index]);
            };

            $scope.toggleCollapse = function (categoryName) {
                if (categoryName !== $scope.activeElement)
                    $scope.activeElement = categoryName;
                else
                    $scope.activeElement = "";
                // collapsed sidebar doesn't toggle drodopwn
                //if ( angular.element('body').hasClass('aside-collapsed')) return true;

                //// make sure the item index exists
                //if( angular.isDefined( collapseList[$index] ) ) {
                //  if ( ! $scope.lastEventFromChild ) {
                //    collapseList[$index] = !collapseList[$index];
                //    closeAllBut($index);
                //  }
                //}
                //else if ( isParentItem ) {
                //  closeAllBut(-1);
                //}

                //$scope.lastEventFromChild = isChild($index);

                //return true;

            };

            // Controller helpers
            // ----------------------------------- 

            // Check item and children active state
            function isActive(item) {

                if (!item) return;

                if (!item.sref || item.sref === '#') {
                    var foundActive = false;
                    angular.forEach(item.submenu, function (value) {
                        if (isActive(value)) foundActive = true;
                    });
                    return foundActive;
                }
                else {
                    return window.location.href.indexOf($state.href(item.sref)) > -1
                }
            }

            function closeAllBut(index) {
                index += '';
                for (var i in collapseList) {
                    if (index < 0 || index.indexOf(i) < 0)
                        collapseList[i] = true;
                }
            }

            function isChild($index) {
                /*jshint -W018*/
                return (typeof $index === 'string') && !($index.indexOf('-') < 0);
            }

        } // activate
    }

})();
