using System;
using System.IO;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Mvc;
using Avelango.Localisation;
using Castle.MicroKernel.Registration;
using Castle.Windsor;

namespace Avelango.Web
{
    public class MvcApplication : HttpApplication
    {
        private static IWindsorContainer _windsorContainer;

        private static void InstallWindsorContainer()
        {
            _windsorContainer = new WindsorContainer();
            _windsorContainer.Install(new WebEntityInstaller());
            _windsorContainer.Register(Classes.FromThisAssembly().BasedOn<IController>().LifestyleTransient());
        }


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            InstallWindsorContainer();
        }


        protected void Application_End() {
            _windsorContainer.Dispose();
        }


        protected void Application_Error(object sender, EventArgs e) {
            var lines = Server.GetLastError().Message.Split('\'', '\'');
            if (lines.Length > 1) {
                if (Path.HasExtension(lines[1])) return;
            }
            ShowCustomErrorPage(Server.GetLastError());
        }


        private void ShowCustomErrorPage(Exception exception) {
            var httpException = exception as HttpException ?? new HttpException(500, "Internal Server Error", exception);
            Response.Clear();
            string action;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("fromAppErrorEvent", true);

            switch (httpException.GetHttpCode()) {
                case 403: action = "Error403"; break;
                case 404: action = "Error404"; break;
                case 500: action = "Error500"; break;
                default: action = "Error?errorData=" + exception.Message; break;
            }
            // Avoid IIS7 getting in the middle
            Response.TrySkipIisCustomErrors = true;
            Server.ClearError();
            Response.Redirect($"~/Error/{action}");
        }
    }
}
