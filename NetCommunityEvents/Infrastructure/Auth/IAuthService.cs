namespace NetCommunityEvents.Infrastructure.Auth
{
    public interface IAuthService
    {
        void SignIn(string name, bool persistent);
        void SignOut();
    }
}