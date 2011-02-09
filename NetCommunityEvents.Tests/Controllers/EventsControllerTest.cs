using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Data;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetCommunityEvents.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTest 
    {
        protected IDataRepository<Appointment> DataRepository;
        protected EventsController Sut;

        protected IEnumerable<Appointment> Appointments;

        [SetUp]
        public void Setup()
        {
            var generetor = new RandomGenerator();
            Appointments = Builder<Appointment>.CreateListOfSize(50)
                .WhereAll()
                .Have(
                    a =>
                    a.StartDate =
                    generetor.Next(January.The(1).AddSeconds(1),
                                   December.The(31).AddHours(23).AddMinutes(59).AddSeconds(59)))
                .And(a => a.EndDate = a.StartDate.AddHours(generetor.Next(1, 36)))
                .Build();

            DataRepository = MockRepository.GenerateStub<IDataRepository<Appointment>>();
            Sut = new EventsController(DataRepository);
        }

        [Test]
        public void Index()
        {
            // Arrange
            DataRepository
                .Stub(d => d.SelectEntities(10, a => a.StartDate >= DateTime.Today, a => a.StartDate))
                .Return(Appointments
                            .Where(a => a.StartDate >= DateTime.Today)
                            .OrderBy(a => a.StartDate)
                            .Take(10))
                .IgnoreArguments();

            DataRepository
                .Stub(d => d.SelectEntities(a => true))
                .Return(Appointments.Where(a => true))
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Index() as ViewResult;
            var viewModel = viewResult.Model as EventsViewModel;
            
            // Assert
            Assert.That(viewModel.Appointments, Is.Not.Null);
            Assert.That(viewModel.Appointments.Count(), Is.EqualTo(10));
            foreach (var appointment in viewModel.Appointments)
            {
                Assert.That(appointment.StartDate, Is.GreaterThanOrEqualTo(DateTime.Today));
            }

            Assert.That(viewModel.AllAppointmentsLength, Is.GreaterThanOrEqualTo(50));
        }

        [Test]
        public void Event()
        {
            // Arrange
            var id = new Guid("00000000-0000-0000-0000-00000000002a");
            DataRepository
                .Stub(d => d.SelectEntity(a => a.Id == id))
                .Return(Appointments.Where(a => a.Id == id).FirstOrDefault())
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Event(id) as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel.Id, Is.EqualTo(id));
        }

        [Test]
        public void AddGet()
        {
            // Arrange

            // Act
            var viewResult = Sut.Add() as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void AddPost()
        {
            // Arrange

            // Act
            var result = Sut.Add(new EventViewModel
                                            {
                                                Title = "Neue Veranstaltung",
                                                Description = "Beschreibung...",
                                                StartDate = DateTime.Now,
                                                EndDate = DateTime.Now.AddHours(4)
                                            }) as RedirectToRouteResult;

            var actionName = result.RouteValues["action"];
            var modelId = result.RouteValues["id"];

            // Assert
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Event"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
        }

        [Test]
        public void EditGet()
        {
            // Arrange
            var id = new Guid("00000000-0000-0000-0000-00000000002a");
            DataRepository
                .Stub(d => d.SelectEntity(a => a.Id == id))
                .Return(Appointments.Where(a => a.Id == id).FirstOrDefault())
                .IgnoreArguments();

            // Act
            var viewResult = Sut.Edit(id) as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(id));
        }

        [Test]
        public void EditPost()
        {
            // Arrange
            var id = new Guid("00000000-0000-0000-0000-00000000002a");
            var viewModel = new EventViewModel
                                {
                                    Title = "Neue Veranstaltung",
                                    Description = "Beschreibung...",
                                    StartDate = DateTime.Now,
                                    EndDate = DateTime.Now.AddHours(4)
                                };

            // Act
            var viewResult = Sut.Edit(id, viewModel) as RedirectToRouteResult;
            var actionName = viewResult.RouteValues["action"];
            var modelId = viewResult.RouteValues["id"];
            
            // Assert
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Event"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
            Assert.That(modelId, Is.EqualTo(id));
        }

        [Test]
        public void Delete()
        {
            // Arrange
            var id = new Guid("00000000-0000-0000-0000-00000000002a");

            // Act
            var viewResult = Sut.Delete(id) as RedirectToRouteResult;
            var actionName = viewResult.RouteValues["action"];
            var modelId = viewResult.RouteValues["id"];

            // Assert
            Assert.That(actionName, Is.Not.Null.Or.Empty);
            Assert.That(actionName, Is.EqualTo("Index"));
            Assert.That(modelId, Is.Not.Null.Or.Empty);
            Assert.That(modelId, Is.EqualTo(id));
        }
    }
}
