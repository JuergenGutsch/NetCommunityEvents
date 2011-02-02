using System;
using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.Dates;
using NetCommunityEvents.Models;
using XmlRepository.DataProviders;

namespace NetCommunityEvents.Data
{
    public class DataRepository<T> where T : Identity, new()
    {
        public DataRepository()
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


        public IEnumerable<T> SelectEntities(Func<T, bool> expression)
        {
            IEnumerable<T> appointments;
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                appointments = repository.LoadAllBy(expression);
            }
            return appointments;
        }

        public IEnumerable<T> SelectEntities(int top, Func<T, bool> expression, Func<T, object> orderby)
        {
            IEnumerable<T> appointments;
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                appointments = repository
                    .LoadAllBy(expression)
                    .OrderBy(orderby)
                    .Take(top);
            }
            return appointments;
        }

        public T SelectEntity(Func<T, bool> expression, Func<T, object> orderby)
        {
            T appointment;
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                appointment = repository
                    .LoadAllBy(expression)
                    .OrderBy(orderby)
                    .FirstOrDefault();
            }
            return appointment;
        }
    }
}