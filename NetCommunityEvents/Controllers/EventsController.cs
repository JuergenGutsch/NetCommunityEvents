﻿using System;
using System.Linq;
using System.Web.Mvc;
using NetCommunityEvents.Data;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;

namespace NetCommunityEvents.Controllers
{
    public class EventsController : Controller
    {
        private readonly IDataRepository<Appointment> _dataRepository;

        public EventsController(IDataRepository<Appointment> dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public ActionResult Index()
        {
            var viewModel = new EventsViewModel();

            viewModel.Appointments = _dataRepository.SelectEntities(10, a => a.StartDate >= DateTime.Today, a => a.StartDate);
            viewModel.AllAppointmentsLength = _dataRepository.SelectEntities(a => true).Count();

            return View(viewModel);
        }


        public ActionResult Event(Guid id)
        {
            var model = _dataRepository.SelectEntity(a => a.Id == id);

            var viewModel = EventViewModel.Create(model);

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View(new EventViewModel());
        }

        [HttpGet]
        public ActionResult Add(EventViewModel viewModel)
        {
            var model = viewModel.CreateModel();

            if(model.Id == Guid.Empty)
            {
                model.Id = Guid.NewGuid();
            }

            _dataRepository.SaveEntity(model);

            return RedirectToAction("Event", new {Id = model.Id});
        }

        public ActionResult Edit(Guid id)
        {
            var model = _dataRepository.SelectEntity(a => a.Id == id);

            var viewModel = EventViewModel.Create(model);

            return View(viewModel);
        }
    }
}
