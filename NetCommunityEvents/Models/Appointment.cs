using System;

namespace NetCommunityEvents.Models
{
    public class Appointment : Identity
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Organizer { get; set; }

        public string Location { get; set; }

        public string Address { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public string Website { get; set; }

        public string RegistrationUrl { get; set; }

        public string ContactEmail { get; set; }

        public string PublisherName { get; set; }

        public string PublisherEmail { get; set; }
    }

    public class Identity
    {
        public Guid Id { get; set; }
    }
}