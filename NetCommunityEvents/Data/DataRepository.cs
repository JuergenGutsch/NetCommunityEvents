﻿using System;
using System.Collections.Generic;
using System.Linq;
using NetCommunityEvents.Models;

namespace NetCommunityEvents.Data
{
    public class DataRepository<T> : IDataRepository<T> where T : Identity, new()
    {
        public IEnumerable<T> SelectEntities(Func<T, bool> expression)
        {
            IEnumerable<T> appointments;
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                appointments = repository.LoadAllBy(expression);
            }
            return appointments ?? new List<T>();
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
            return appointments ?? new List<T>();
        }

        public T SelectEntity(Func<T, bool> expression)
        {
            T appointment;
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                appointment = repository
                    .LoadAllBy(expression)
                    .FirstOrDefault();
            }
            return appointment;
        }

        public void SaveEntity(T model)
        {
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                repository.SaveOnSubmit(model);
            }
        }

        public void DelelteEntity(Func<T, bool> expression)
        {
            using (var repository = XmlRepository.XmlRepository.GetInstance<T>())
            {
                repository.DeleteOnSubmit(expression);
            }
        }
    }
}