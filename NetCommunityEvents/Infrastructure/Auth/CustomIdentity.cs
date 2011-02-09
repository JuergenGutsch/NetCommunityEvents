using System.Security.Principal;
using System.Web;
using LightCore;
using LightCore.Integration.Web;
using NetCommunityEvents.Data;
using NetCommunityEvents.Models;

namespace NetCommunityEvents.Infrastructure.Auth
{
    public class CustomIdentity : IIdentity
    {
        private User _user;

        public IIdentity ParentIdentity { get; private set; }

        public string AuthenticationType { get; private set; }

        public string Name { get; private set; }

        private IDataRepository<User> UserService
        {
            get
            {
                IContainer container = ((IContainerAccessor)HttpContext.Current.ApplicationInstance).Container;
                return container.Resolve<IDataRepository<User>>();
            }
        }

        public bool IsAuthenticated
        {
            get { return User != null && !HttpContext.Current.User.IsInRole("Guest"); }
        }

        public User User
        {
            get
            {
                if (_user == null)
                {
                    _user = UserService.SelectEntity(u=> u.Name ==  Name);
                }
                return _user;
            }
        }

        public CustomIdentity(IIdentity identity)
        {
            ParentIdentity = identity;
            AuthenticationType = identity.AuthenticationType;
            Name = identity.Name;
        }
        public CustomIdentity(string name, string authType)
        {
            AuthenticationType = authType;
            Name = name;
        }

    }
}