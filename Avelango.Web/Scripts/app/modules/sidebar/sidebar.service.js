(function() {
    'use strict';

    angular
        .module('app.sidebar')
        .service('SidebarLoader', SidebarLoader);

    function SidebarLoader() {
       this.getMenu = getMenu;
       
        ////////////////

        function getMenu(onReady) {
          

            var data= [
                {
                    "text": "USERS",
                    "sref": "userss",
                    "translate": "USERS",
                    "icon": "icon-user"
                },
                {
                    "text": "MODERATORS",
                    "sref": "moderatorss",
                    "icon": "icon-speedometer",
                    "label": "label label-info",
                    "translate": "MODERATORS"
                },
                {
                    "text": "STATISTIC",
                    "sref": "app.widgets",
                    "icon": "icon-grid",
                    "label": "label label-info",
                    "translate": "STATISTIC"
                },
                {
                    "text": "LOGI",
                    "sref": "app.widgets",
                    "icon": "icon-puzzle",
                    "label": "label label-info",
                    "translate": "LOGI"
                }
             
            ];
            onReady(data);
            }
        }
})();