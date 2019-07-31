using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountServiceShould
    {
        private readonly Mock<IDataStoreFactory> _dataStoreFactory;
        private readonly Mock<IAppConfiguration> _appConfiguration;
        private AccountService _accountService;
        private readonly Mock<IAccountDataStore> _dataStore;

        public AccountServiceShould()
        {
            _dataStoreFactory = new Mock<IDataStoreFactory>();
            _appConfiguration = new Mock<IAppConfiguration>();
            _dataStore = new Mock<IAccountDataStore>();
            _dataStoreFactory.Setup(x => x.CreateDataStore(It.IsAny<string>())).Returns(
                () => _dataStore.Object);
        }

        [Fact]
        public void GetAnAccountFromAccountDataStore()
        {
            _accountService = new AccountService(_dataStoreFactory.Object, _appConfiguration.Object);

            _accountService.GetAccount(string.Empty);

            _dataStore.Verify(x => x.GetAccount(string.Empty), Times.Once);
        }

        [Fact]
        public void ReduceBalanceOnTheAccount_WhenUpdatingTheAccount()
        {
            var account = new Account { Balance = 2 };
            var makePaymentRequest = new MakePaymentRequest { Amount = 1 };
            _accountService = new AccountService(_dataStoreFactory.Object, _appConfiguration.Object);

            _accountService.UpdateAccount(account, makePaymentRequest.Amount);

            account.Balance.Should().Be(1);
        }

        [Fact]
        public void UpdateAccountOnDataStor_WhenUpdatingTheAccounte()
        {
            var account = new Account { Balance = 2 };
            var makePaymentRequest = new MakePaymentRequest { Amount = 1 };
            _accountService = new AccountService(_dataStoreFactory.Object, _appConfiguration.Object);

            _accountService.UpdateAccount(account, makePaymentRequest.Amount);

            _dataStore.Verify(x => x.UpdateAccount(It.Is<Account>(a => a.Balance == 1)));
        }
    }
}