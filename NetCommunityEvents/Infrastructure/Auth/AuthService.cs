using System.Web.Security;

namespace NetCommunityEvents.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        public void SignIn(string name, bool persistent)
        {
            FormsAuthentication.SetAuthCookie(name, persistent);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}