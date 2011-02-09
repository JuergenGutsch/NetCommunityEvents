using System;
using System.Web;
using System.Web.Security;
using LightCore;
using LightCore.Integration.Web;
using NetCommunityEvents.Data;
using NetCommunityEvents.Infrastructure;
using NetCommunityEvents.Infrastructure.Auth;
using NetCommunityEvents.Models;

namespace NetCommunityEvents
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication, IContainerAccessor
    {
        public IContainer Container { get; set; }

        protected void Application_Start()
        {
            SetupDiContainer();
            
            Bootstrapper.CreateNew()
                .Execute();
        }

        private void SetupDiContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IDataRepository<Appointment>, DataRepository<Appointment>>();
            builder.Register<IDataRepository<User>, DataRepository<User>>();
            builder.Register<IDataRepository<Content>, DataRepository<Content>>();

            builder.Register<IBootstrapItem, AreasInitializer>();
            builder.Register<IBootstrapItem, ControllerFactoryInitializer>();
            builder.Register<IBootstrapItem, GlobalFiltersInitializer>();
            builder.Register<IBootstrapItem, RoutesInitializer>();
            builder.Register<IBootstrapItem, DataStoreInitializer>();

            Container = builder.Build();
        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            string name = String.Empty;
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                name = authTicket.Name;
            }
            var identity = new CustomIdentity(name, "Forms");
            var principal = new CustomPrincipal(identity);
            Context.User = principal;
        }


    }
}