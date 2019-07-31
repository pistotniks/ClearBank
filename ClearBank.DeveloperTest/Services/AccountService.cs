using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountDataStore _dataStore;

        public AccountService(
            IDataStoreFactory dataStoreFactory,
            IAppConfiguration appConfiguration)
        {
            _dataStore = dataStoreFactory.CreateDataStore(appConfiguration.DataStoreType);
        }

        public Account GetAccount(string accountNumber)
        {
            return _dataStore.GetAccount(accountNumber);
        }

        public void UpdateAccount(Account account, decimal requestedAmount)
        {
            account.Deduct(requestedAmount);

            _dataStore.UpdateAccount(account);
        }
    }
}