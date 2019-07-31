namespace ClearBank.DeveloperTest.Services
{
    public interface IDataStoreFactory
    {
        IAccountDataStore CreateDataStore(string dataStoreType);
    }
}