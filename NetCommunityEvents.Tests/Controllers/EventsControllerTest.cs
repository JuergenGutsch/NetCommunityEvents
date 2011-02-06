using System;
using System.Linq;
using System.Web.Mvc;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace NetCommunityEvents.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTest : EventsTestBase
    {
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
            var viewResult = EventsController.Index() as ViewResult;
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
            var viewResult = EventsController.Event(id) as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel.Id, Is.EqualTo(id));
        }

        [Test]
        public void AddGet()
        {
            // Arrange

            // Act
            var viewResult = EventsController.Add() as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void AddPost()
        {
            // Arrange

            // Act
            var result = EventsController.Add(new EventViewModel
                                            {
                                                Title = "Neue Veranstaltung",
                                                Description = "Beschreibung...",
                                                StartDate = DateTime.Now,
                                                EndDate = DateTime.Now.AddHours(4)
                                            }) as RedirectToRouteResult;

            var actionName = result.RouteValues["action"];
            var modelId = result.RouteValues["action"];

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
            var viewResult = EventsController.Edit(id) as ViewResult;
            var viewModel = viewResult.Model as EventViewModel;

            // Assert
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Id, Is.EqualTo(id));
        }
    }
}
