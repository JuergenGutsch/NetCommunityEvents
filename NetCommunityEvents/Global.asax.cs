using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using NetCommunityEvents.Models;
using XmlRepository.DataProviders;

namespace NetCommunityEvents
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new {controller = "Home", action = "Index", id = UrlParameter.Optional} // Parameter defaults
                );
        }

        protected void Application_BeginRequest()
        {
            XmlRepository.XmlRepository.DefaultQueryProperty = "Id";
            XmlRepository.XmlRepository.DataProvider = new XmlInMemoryProvider();

            var generetor = new RandomGenerator();
            var appointments = Builder<Appointment>.CreateListOfSize(50)
                .WhereAll()
                    .Have(a => a.StartDate = generetor.Next(January.The(1).AddSeconds(1), December.The(31).AddHours(23).AddMinutes(59).AddSeconds(59)))
                    .And(a => a.EndDate = a.StartDate.AddHours(generetor.Next(1, 36)))
                .Build();

            using (var repository = XmlRepository.XmlRepository.GetInstance<Appointment>())
            {
                repository.SaveOnSubmit(appointments);
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
}
    }
}