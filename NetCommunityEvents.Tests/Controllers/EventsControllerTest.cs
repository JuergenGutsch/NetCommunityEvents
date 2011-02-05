using System;
using System.Linq;
using System.Web.Mvc;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Models;
using NetCommunityEvents.ViewModels;
using NUnit.Framework;
using XmlRepository.DataProviders;

namespace NetCommunityEvents.Tests.Controllers
{
    [TestFixture]
    public class EventsControllerTest
    {
        #region Setup

        [SetUp]
        public void Setup()
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

        #endregion

        [Test]
        public void Index()
        {
            // Arrange
            var controller = new EventsController();

            // Act
            var result = controller.Index() as ViewResult;

            var model = result.Model as EventsViewModel;
            
            // Assert
            Assert.That(model.Appointments, Is.Not.Null);
            Assert.That(model.Appointments.Count(), Is.EqualTo(10));
            foreach (var appointment in model.Appointments)
            {
                Assert.That(appointment.StartDate, Is.GreaterThanOrEqualTo(DateTime.Today));
            }

            Assert.That(model.AllAppointmentsLength, Is.EqualTo(50));
        }

        [Test]
        public void Event()
        {
            // Arrange
            var controller = new EventsController();

            // Act
            var result = controller.Event(new Guid("00000000-0000-0000-0000-00000000002a")) as ViewResult;

            var model = result.Model as EventViewModel;

            // Assert
            Assert.That(model.Id, Is.EqualTo(new Guid("00000000-0000-0000-0000-00000000002a")));
        }
    }
}
