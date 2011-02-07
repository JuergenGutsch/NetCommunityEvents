using System.Web;
using XmlRepository.DataProviders;

namespace NetCommunityEvents.Infrastructure
{
    public class DataStoreInitializer : IBootstrapItem
    {
        public void Execute()
        {
            XmlRepository.XmlRepository.DefaultQueryProperty = "Id";
            XmlRepository.XmlRepository.DataProvider = new XmlFileProvider(HttpContext.Current.Server.MapPath("~/App_Data/"));
        }
    }
}