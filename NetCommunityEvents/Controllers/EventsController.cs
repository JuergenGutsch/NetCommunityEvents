using System;
using System.Web.Mvc;
using NetCommunityEvents.Data;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;

namespace NetCommunityEvents.Controllers
{
    public class EventsController : Controller
    {
        //
        // GET: /Events/

        public ActionResult Index()
        {
            var viewModel = new EventsViewModel();
            var repository = new DataRepository<Appointment>();

            viewModel.Appointments = repository.SelectEntities(10, a => a.StartDate >= DateTime.Today, a => a.StartDate);

            return View(viewModel);
        }

    }
}
