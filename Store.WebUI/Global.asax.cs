using Store.Domain.Entities;
using Store.WebUI.App_Start;
using Store.WebUI.Infrastructure.Binders;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Store.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            ModelBinders.Binders.Add(typeof(Cart), new CartModelBinder()); ///binding cart instance from(to) seession
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
