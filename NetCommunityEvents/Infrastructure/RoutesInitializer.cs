using System.Web.Mvc;
using System.Web.Routing;

namespace NetCommunityEvents.Infrastructure
{
    public class RoutesInitializer : IBootstrapItem
    {
        public void Execute()
        {
            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );
        }
    }
}