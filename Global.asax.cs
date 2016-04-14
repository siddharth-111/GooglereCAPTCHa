using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace GooglereCAPTCHa
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            // Administrator will only be allowed a certain number of login attempts
            Session["MaxLoginAttempts"] = 2;
            Session["LoginCount"] = 0;

            // Track whether they're logged in or not
            Session["LoggedIn"] = "No";
        }
    }
}
