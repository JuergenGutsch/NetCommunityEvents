using System.Web.Mvc;

namespace NetCommunityEvents.Infrastructure
{
    public class AreasInitializer : IBootstrapItem
    {
        public void Execute()
        {
            AreaRegistration.RegisterAllAreas();
        }
    }
}