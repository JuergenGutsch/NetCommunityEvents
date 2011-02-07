using System.Web;
using System.Web.Mvc;
using LightCore.Integration.Web;
using LightCore.Integration.Web.Mvc;

namespace NetCommunityEvents.Infrastructure
{
    public class ControllerFactoryInitializer : IBootstrapItem
    {
        public void Execute()
        {
            IContainerAccessor accessor = (IContainerAccessor)HttpContext.Current.ApplicationInstance;
            var container = accessor.Container;
            ControllerBuilder.Current.SetControllerFactory(new ControllerFactory(container));
        }
    }
}