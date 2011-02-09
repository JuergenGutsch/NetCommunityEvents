using NetCommunityEvents.Controllers;

namespace NetCommunityEvents.Models
{
    public class User : Identity
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsValid { get; set; }

        public Role Role { get; set; }
    }
}