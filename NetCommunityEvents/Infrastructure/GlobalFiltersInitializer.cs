using System.Web.Mvc;

namespace NetCommunityEvents.Infrastructure
{
    public class GlobalFiltersInitializer : IBootstrapItem
    {
        public void Execute()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }
    }
}