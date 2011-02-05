using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using NetCommunityEvents.Data;
using NetCommunityEvents.Models;
using NUnit.Framework;
using XmlRepository.DataProviders;

namespace NetCommunityEvents.Tests.Data
{
    [TestFixture]
    public class DataRepositoryTests
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
        public void SelectEntities()
        {
            var sut = new DataRepository<Appointment>();
            var appointments = sut.SelectEntities(a => a.StartDate >= DateTime.Today);

            Assert.That(appointments, Is.Not.Null);
            Assert.That(appointments.Count(), Is.GreaterThan(0));
            foreach (var appointment in appointments)
            {
                Assert.That(appointment.StartDate, Is.GreaterThanOrEqualTo(DateTime.Today));
            }
        }

        [Test]
        public void SelectNext10EntitiesOrderedByStartDate()
        {
            var sut = new DataRepository<Appointment>();
            var appointments = sut.SelectEntities(10, a => a.StartDate >= DateTime.Today, a => a.StartDate);

            Assert.That(appointments, Is.Not.Null);
            Assert.That(appointments.Count(), Is.EqualTo(10));
            foreach (var appointment in appointments)
            {
                Assert.That(appointment.StartDate, Is.GreaterThanOrEqualTo(DateTime.Today));
            }
        }

        [Test]
        public void SelectTheNewestEntity()
        {
            var sut = new DataRepository<Appointment>();
            var appointment = sut.SelectEntity(a => a.StartDate >= DateTime.Today);

            Assert.That(appointment, Is.Not.Null);
            Assert.That(appointment.StartDate, Is.GreaterThanOrEqualTo(DateTime.Today));
        }

    }
}
