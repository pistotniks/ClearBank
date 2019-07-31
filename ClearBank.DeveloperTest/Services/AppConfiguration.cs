using System.Configuration;

namespace ClearBank.DeveloperTest.Services
{
    public class AppConfiguration : IAppConfiguration
    {
        public string DataStoreType
        {
            get
            {
                var value = ConfigurationManager.AppSettings["DataStoreType"];
                // TODO return a concrete type class DataStore
                return string.IsNullOrEmpty(value) ? string.Empty : value;
            }
        }
    }
}