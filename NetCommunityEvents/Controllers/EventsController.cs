using System;
using System.Linq;
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
            viewModel.AllAppointmentsLength = repository.SelectEntities(a => true).Count();

            return View(viewModel);
        }


        public ActionResult Event(Guid id)
        {
            var repository = new DataRepository<Appointment>();

            var model = repository.SelectEntity(a => a.Id == id);

            var viewModel = EventViewModel.Create(model);

            return View(viewModel);
        }
    }
}
