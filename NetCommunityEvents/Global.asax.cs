using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LightCore;
using LightCore.Integration.Web;
using LightCore.Integration.Web.Mvc;
using NetCommunityEvents.Data;
using NetCommunityEvents.Infrastructure;
using NetCommunityEvents.Models;
using XmlRepository.DataProviders;

namespace NetCommunityEvents
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        protected void Application_Start()
        {
            SetupDiContainer();
            
            Bootstrapper.Start()
                .FromServiceLocator()
                .Execute();
        }

        private void SetupDiContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IDataRepository<Appointment>, DataRepository<Appointment>>();

            builder.Register<IBootstrapItem, AreasInitializer>();
            builder.Register<IBootstrapItem, ControllerFactoryInitializer>();
            builder.Register<IBootstrapItem, GlobalFiltersInitializer>();
            builder.Register<IBootstrapItem, RoutesInitializer>();
            builder.Register<IBootstrapItem, DataStoreInitializer>();

            Container = builder.Build();
        }

        public IContainer Container { get; set; }
    }
}