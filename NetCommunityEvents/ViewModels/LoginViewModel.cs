using System;

namespace NetCommunityEvents.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Persistent { get; set; }
    }
}