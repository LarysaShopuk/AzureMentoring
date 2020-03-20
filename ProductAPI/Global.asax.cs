using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ProductAPI.Services;
using Unity;

namespace ProductAPI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var unity = new UnityContainer();
            RegisterDependencies(unity);

            GlobalConfiguration.Configuration.DependencyResolver = new UnityResolver(unity);
        }

        private void RegisterDependencies(UnityContainer container)
        {
            ServicesDependencyInitializer.RegisterDependencies(container);
        }
    }
}
