using ClearBank.DeveloperTest.Data;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountDataStoreFactory : IDataStoreFactory
    {
        public IAccountDataStore CreateDataStore(string dataStoreType)
        {
            if (dataStoreType == Resources.Backup)
            {
                return new BackupAccountDataStore();
            }

            return new AccountDataStore();
        }
    }
}