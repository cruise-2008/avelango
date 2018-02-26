using System.Web.Optimization;

namespace Avelango
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //var googlelibrary = "https://maps.googleapis.com/maps/api/js?key=AIzaSyCc0AVvpTx3fEdFnkqMHyn3donBqPF1ouI&libraries=places";

            // STYLES --------------------------------------------
            bundles.Add(new StyleBundle("~/bundles/custom")
              .Include("~/Content/custom/huckfin.css")
              .Include("~/Content/custom/main.css")
              .Include("~/Content/custom/inner.css")
              .Include("~/Content/custom/icons.data.svg.css")
              .Include("~/Content/custom/maintext.css")
              .Include("~/Content/custom/ui-kit.css")
              .Include("~/Content/custom/scroll.css")
              .Include("~/Vendor/sweetAlert/dist/sweetAlert.css")
              .Include("~/Vendor/angularjs-toaster/toaster.css")
              .Include("~/Vendor/jscrollpane/jquery.jscrollpane.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrapStyles")
                    .Include("~/Content/app/bootstrap.css"));

            //bundles.Add(new StyleBundle("~/bundles/appStyles")
            //        .Include("~/Content/app/css/theme-a.css")
            //        .Include("~/Content/app/app.css")
            //        .Include("~/Content/mvc-override.css"));
            //
            //bundles.Add(new StyleBundle("~/bundles/custom")
            //        //.Include("~/Content/custom/main.css")
            //        .Include("~/Content/custom/vstyle.css")
            //        //.Include("~/Content/custom/home.css")
            //        .Include("~/Vendor/sweetAlert/dist/sweetAlert.css")
            //        .Include("~/Vendor/angularjs-toaster/toaster.css"));

            bundles.Add(new StyleBundle("~/bundles/appStyles")
                    .Include("~/Content/app/css/font-awesome.css"));

      

            // JS -----------------------------------------------
            bundles.Add(new ScriptBundle("~/bundles/startScripts")
                    .Include("~/Vendor/jquery/dist/jquery.js")
                    .Include("~/Vendor/angular/angular.js"));


            // Base Scripts
            bundles.Add(new ScriptBundle("~/bundles/baseScripts").Include(
              "~/Vendor/angular-signalr/signalr-hub.js",
              "~/Vendor/angular-route/angular-route.js",
              "~/Vendor/angular-cookies/angular-cookies.js",
              "~/Vendor/angular-animate/angular-animate.js",
              "~/Vendor/angular-touch/angular-touch.js",
              "~/Vendor/angular-ui-router/release/angular-ui-router.js",
              "~/Vendor/ngstorage/ngStorage.js",
              "~/Vendor/angular-ui-event/dist/event.js",
              "~/Vendor/angular-ui-validate/dist/validate.js",
              "~/Vendor/angular-ui-scroll/dist/ui-scroll.js",
              "~/Vendor/angular-sanitize/angular-sanitize.js",
              "~/Vendor/sweetAlert/dist/sweetAlert.min.js",
              "~/Vendor/angular-sweetAlert/sweetAlert.js",
              "~/Vendor/angular-ui-router/release/angular-ui-router.js",
              "~/Vendor/angular-resource/angular-resource.js",
              "~/Vendor/angular-translate/angular-translate.js",
              "~/Vendor/angular-translate-loader-url/angular-translate-loader-url.js",
              "~/Vendor/angular-translate-loader-static-files/angular-translate-loader-static-files.js",
              "~/Vendor/angular-translate-storage-local/angular-translate-storage-local.js",
              "~/Vendor/angular-translate-storage-cookie/angular-translate-storage-cookie.js",
              "~/Vendor/oclazyload/dist/ocLazyLoad.js",
              "~/Vendor/angular-bootstrap/ui-bootstrap-tpls.js",
              "~/Vendor/angular-loading-bar/build/loading-bar.js",
              "~/Vendor/angular-dynamic-locale/dist/tmhDynamicLocale.js",
              "~/Vendor/jquery.browser/dist/jquery.browser.js",
              "~/Vendor/angularjs-toaster/toaster.js",
              //"~/Vendor/Flot/jquery.flot.js",
              //"~/Vendor/Flot/jquery.flot.categories.js",
              //"~/Vendor/Flot/jquery.flot.pie.js",
              //"~/Vendor/Flot/jquery.flot.resize.js",
              //"~/Vendor/Flot/jquery.flot.time.js",
              //"~/Vendor/flot-spline/js/jquery.flot.spline.min.js",
              //"~/Vendor/flot.tooltip/js/jquery.flot.tooltip.min.js",
              "~/Vendor/highchart/highstock.js",
              "~/Vendor/jscrollpane/jquery.jscrollpane.js"
            ));


            // SignalR
            bundles.Add(new ScriptBundle("~/bundles/signalR").Include("~/Scripts/jquery.signalR-2.2.0.min.js"));


            // APP
            bundles.Add(new ScriptBundle("~/bundles/appScripts")
                .Include("~/Scripts/app/modules/extras/extras.module.js")
                .Include("~/Scripts/app/app.module.js")






                //parlour controllers start 
                .Include("~/Scripts/app/parlourControllers/00_GlobalParlourCtrl.js")
                .Include("~/Scripts/app/parlourControllers/01_StocksAndNewsCtrl.js")
                .Include("~/Scripts/app/parlourControllers/02_UserInfoCtrl.js")
                .Include("~/Scripts/app/parlourControllers/03_PortfolioCtrl.js")
                .Include("~/Scripts/app/parlourControllers/04_UserTaskCtrl.js")
                .Include("~/Scripts/app/parlourControllers/05_UserExecutesCtrl.js")
                .Include("~/Scripts/app/parlourControllers/06_UserGroupCtrl.js")
                .Include("~/Scripts/app/parlourControllers/07_UserMoneyCtrl.js")
                .Include("~/Scripts/app/parlourControllers/08_UserFavoritesCtrl.js")
                .Include("~/Scripts/app/parlourControllers/09_UserReviewsCtrl.js")
                .Include("~/Scripts/app/parlourControllers/10_UserExaminationsCtrl.js")
                .Include("~/Scripts/app/parlourControllers/11_UserAwardsCtrl.js")
                
                //home page controller start    
                .Include("~/Scripts/app/homePageControllers/00_GlobalHomePageCtrl.js")

                //Services
                .Include("~/Scripts/app/service/login.service.js")
                .Include("~/Scripts/app/service/cookie.service.js")
                .Include("~/Scripts/app/service/validator.service.js")
                .Include("~/Scripts/app/service/open.tickets.service.js")
                .Include("~/Scripts/app/service/pagination.service.js")
                .Include("~/Scripts/app/service/timer.jobs.service.js")
                .Include("~/Scripts/app/service/menu.filter.service.js")
                .Include("~/Scripts/app/service/map.location.service.js")
                .Include("~/Scripts/app/service/close.modal.service.js")
                .Include("~/Scripts/app/homePageControllers/service/scroll.home.page.js")
                .Include("~/Scripts/app/service/modalWindow.open.service.js")
                .Include("~/Scripts/app/parlourControllers/parlourService/sizeAndPagination.service.js")

                //Modules
                .Include("~/Scripts/app/custom/layout.module.js")
                .Include("~/Scripts/app/custom/wizard.controller.js")
                .Include("~/Scripts/app/custom/custom.module.js")
                .Include("~/Scripts/app/custom/lang.module.js")
                .Include("~/Scripts/app/custom/notifications.service.js")
                .Include("~/Scripts/app/custom/signalr.service.js")
                .Include("~/Scripts/app/custom/userauth.module.js")
                .Include("~/Scripts/app/custom/userauth.service.js")
                .Include("~/Scripts/app/custom/lang.service.js")
                .Include("~/Scripts/app/custom/custom.controller.js")
                .Include("~/Scripts/app/modules/charts/charts.module.js")
                .Include("~/Scripts/app/modules/pages/rialto.controller.js")
                .Include("~/Scripts/app/modules/panels/panels.controller.js")
                //.Include("~/Scripts/app/modules/pages/mylazyscripts.derectives.js")
                .Include("~/Scripts/app/modules/tables/tables.module.js")
                .Include("~/Scripts/app/modules/sidebar/sidebar.module.js")
                .Include("~/Scripts/app/modules/sidebar/sidebar.controller.js")
                .Include("~/Scripts/app/modules/sidebar/sidebar.service.js")
                //.Include("~/Scripts/app/tables/datatable.service.js")
                //.IncludeDirectory("~/Scripts/app/modules/tables", "*.js", true)
                .Include("~/Scripts/app/modules/core/core.module.js")
                .IncludeDirectory("~/Scripts/app/modules/core", "*.js", true)
                //.Include("~/Scripts/app/modules/colors/colors.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/colors", "*.js", true)
                .Include("~/Scripts/app/modules/lazyload/lazyload.module.js")
                .IncludeDirectory("~/Scripts/app/modules/lazyload", "*.js", true)
                .Include("~/Scripts/app/modules/loadingbar/loadingbar.module.js")
                .IncludeDirectory("~/Scripts/app/modules/loadingbar", "*.js", true)
                //.Include("~/Scripts/app/modules/navsearch/navsearch.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/navsearch", "*.js", true)
                //.Include("~/Scripts/app/modules/preloader/preloader.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/preloader", "*.js", true)
                .Include("~/Scripts/app/modules/routes/routes.module.js")
                .IncludeDirectory("~/Scripts/app/modules/routes", "*.js", true)
                //.Include("~/Scripts/app/modules/settings/settings.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/settings", "*.js", true)
                //.Include("~/Scripts/app/modules/sidebar/sidebar.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/sidebar", "*.js", true)
                .Include("~/Scripts/app/modules/translate/translate.module.js")
                .IncludeDirectory("~/Scripts/app/modules/translate", "*.js", true)
                .Include("~/Scripts/app/modules/utils/utils.module.js")
                .IncludeDirectory("~/Scripts/app/modules/utils", "*.js", true)
                //.Include("~/Scripts/app/modules/dashboard/dashboard.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/dashboard", "*.js", true)
                .Include("~/Scripts/app/modules/charts/charts.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/charts", "*.js", true)
                .Include("~/Scripts/app/modules/elements/elements.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/elements", "*.js", true)
                //.Include("~/Scripts/app/modules/elements/sweetAlert.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/bootstrapui.module.js")
                .Include("~/Scripts/app/modules/bootstrapui/bootstrapui.config.js")
                //.IncludeDirectory("~/Scripts/app/modules/bootstrapui", "*.js", true)
                //.Include("~/Scripts/app/modules/bootstrapui/carousel.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/modals.controller.js")
                .Include("~/Scripts/app/modules/extras/calendar.directive.js")
                //.IncludeDirectory("~/Scripts/app/modules/extras", "*.js", true)
                //.Include("~/Scripts/app/modules/flatdoc/flatdoc.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/flatdoc", "*.js", true)
                .Include("~/Scripts/app/modules/icons/icons.module.js")
                .IncludeDirectory("~/Scripts/app/modules/icons", "*.js", true)
                //.Include("~/Scripts/app/modules/locale/locale.module.js")
                // .IncludeDirectory("~/Scripts/app/modules/locale", "*.js", true)
                //.Include("~/Scripts/app/modules/mailbox/mailbox.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/mailbox", "*.js", true)
                //.Include("~/Scripts/app/modules/maps/maps.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/maps", "*.js", true)
                //.Include("~/Scripts/app/modules/notify/notify.module.js")
                //.IncludeDirectory("~/Scripts/app/modules/notify", "*.js", true)
                .Include("~/Scripts/app/modules/pages/pages.module.js")
                .Include("~/Scripts/app/modules/pages/order-index.controller.js")
                .Include("~/Scripts/app/modules/pages/user-portfolio.js")
                //.Include("~/Scripts/app/modules/pages/main.moderator.controller.js") // ~!~
                //.Include("~/Scripts/app/modules/pages/mylazyscripts.derectives.js")
                //.IncludeDirectory("~/Scripts/app/modules/pages", "*.js", true)
                .Include("~/Scripts/app/modules/pages/myuser.controller.js")
                .Include("~/Scripts/app/modules/pages/myuser.map.controller.js")
                .Include("~/Scripts/app/modules/pages/myuser.derectives.js")
                .Include("~/Scripts/app/modules/panels/panels.module.js")
                //.Include("~/Scripts/app/modules/panels/panel-dismiss.directive.js")
                //.IncludeDirectory("~/Scripts/app/modules/panels", "*.js", true)
                .Include("~/Scripts/app/modules/forms/forms.module.js")
                .IncludeDirectory("~/Scripts/app/modules/forms", "*.js", true)
                .Include("~/Scripts/app/modules/extras/toaster.controller.js")
                .Include("~/Scripts/app/modules/elements/bell.controller.js")
                .Include("~/Scripts/app/modules/elements/sweetAlert.controller.js")
                );

            //Home/Index
            bundles.Add(new ScriptBundle("~/bundles/homeScripts")
                .Include("~/Scripts/app/modules/bootstrapui/datepicker.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/timepicker.controller.js")
                .Include("~/Scripts/app/modules/elements/carousel.controller.js")
                .Include("~/Scripts/app/modules/elements/sweetAlert.controller.js"));//убрать в дальнейшем

           
             bundles.Add(new ScriptBundle("~/bundles/tasksScripts")
                 .Include("~/Scripts/app/modules/bootstrapui/pagination.controller.js")
                 .Include("~/Scripts/app/modules/elements/sweetAlert.controller.js")
                 .Include("~/Scripts/app/modules/pages/tasks.controller.js")
                 .Include("~/Scripts/app/modules/elements/stars.service.js"));

            //
            bundles.Add(new ScriptBundle("~/bundles/executorsScripts")
                .Include("~/Scripts/app/modules/elements/sweetAlert.controller.js")
                .Include("~/Scripts/app/modules/pages/executors.controller.js")
                .Include("~/Scripts/app/modules/elements/stars.service.js"));

            //
            bundles.Add(new ScriptBundle("~/bundles/userParlourScripts")
                .Include("~/Scripts/app/modules/elements/sweetAlert.controller.js")
                .Include("~/Scripts/app/modules/pages/myuser.derectives.js")
                .Include("~/Scripts/app/modules/pages/myuser.controller.js")
                .Include("~/Scripts/app/modules/tables/datatable.users.controller.js")
                .Include("~/Scripts/app/modules/tables/datatable.myorder.controller.js")
                .Include("~/Scripts/app/modules/pages/myuser.map.controller.js")
                .Include("~/Scripts/app/modules/tables/datatableuserjob.controller.js")
                .Include("~/Scripts/app/modules/tables/datatablemessage.controller.js")
                .Include("~/Scripts/app/modules/tables/datatable.money.controller.js")
                .Include("~/Scripts/app/modules/tables/datatablefavorites.controller.js")
                .Include("~/Scripts/app/modules/tables/datatable.group.controller.js")
                .Include("~/Scripts/app/modules/tables/datatablereviews.controller.js")
                ////.Include("~/Scripts/app/modules/bootstrapui/datepicker.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/timepicker.controller.js")
                .Include("~/Scripts/app/modules/elements/stars.service.js"));

            //
            bundles.Add(new ScriptBundle("~/bundles/moderParlourScripts")
                .Include("~/Scripts/app/modules/pages/main.moderator.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/datepicker.controller.js")
                .Include("~/Scripts/app/modules/bootstrapui/timepicker.controller.js")
                .Include("~/Scripts/app/modules/tables/moderator.orders.controller.js")
                .Include("~/Scripts/app/modules/tables/moderator.users.controller.js"));

            //
            bundles.Add(new ScriptBundle("~/bundles/loginScripts")
                .Include("~/Scripts/app/modules/pages/access-login.controller.js"));
        }
    }
}
