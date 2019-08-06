using Microsoft.AspNet.Identity;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;

namespace TruckCheckUp.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(MvcApplication));

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
              );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
            _logger.Info("Application Started...");
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Response.Clear();
            //Do not display error in browser
            Server.ClearError();
            //Log error
            _logger.Error("Unhandled Exception Application Error" + Environment.NewLine +
           "User : " + HttpContext.Current.User.Identity.GetUserId() + Environment.NewLine +
           "Page : " + HttpContext.Current.Request.Url.AbsoluteUri, exception);

            var httpException = exception as HttpException;

            if (httpException != null)
            {
                if (httpException.GetHttpCode() == 404)
                {
                    Response.RedirectToRoute("Default", new { controller = "ErrorHandler", action = "NotFound" });
                }
                else if (httpException.GetHttpCode() == 500)
                {
                    Response.RedirectToRoute("Default", new { controller = "ErrorHandler", action = "ServerError" });
                }
                else
                {
                    Response.RedirectToRoute("Default", new { controller = "ErrorHandler", action = "Error" });
                }
            }
        }
    }
}
