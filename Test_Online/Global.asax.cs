using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace Test_Online
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
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var memberCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (memberCookie != null)
            {
                var memberTicket = FormsAuthentication.Decrypt(memberCookie.Value);
                var role = memberTicket.UserData.Split(new char[] { ';' });
                var userPrincipal = new GenericPrincipal(new GenericIdentity(memberTicket.Name), role);
                Context.User = userPrincipal;
            }

        }
    }
}
