﻿using System;
using System.Collections.Generic;
using NetCommunityEvents.Models;

namespace NetCommunityEvents.ViewModels
{
    public class EventsViewModel
    {
        public IEnumerable<Appointment> Appointments { get; set; }

        public long AllAppointmentsLength { get; set; }
    }
}