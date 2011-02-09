using System.Security.Principal;
using NetCommunityEvents.Controllers;
using NetCommunityEvents.Infrastructure.Extensions;

namespace NetCommunityEvents.Infrastructure.Auth
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity
        {
            get;
            private set;
        }

        public CustomPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public bool IsInRole(Role role)
        {
            if (((CustomIdentity)Identity).User == null)
            {
                return false;
            }
            return role == ((CustomIdentity)Identity).User.Role;
        }

        public bool IsInRole(string role)
        {
            return IsInRole(role.ToOrDefault<Role>());
        }
    }
}