using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Services;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class AccountDataStoreFactoryShould
    {
        private readonly AccountDataStoreFactory _accountDataStoreFactory;

        public AccountDataStoreFactoryShould()
        {
            _accountDataStoreFactory = new AccountDataStoreFactory();
        }

        [Fact]
        public void CreateBackupAccountDataStore_GivenBackupSettings()
        {
            var accountDataStore = _accountDataStoreFactory.CreateDataStore(Resources.Backup);

            accountDataStore.Should().BeOfType<BackupAccountDataStore>();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Foo")]
        public void CreateAccountDataStore_GivenBackupSettingsIsNotSet(string dataStoreType)
        {
            var accountDataStore = _accountDataStoreFactory.CreateDataStore(dataStoreType);

            accountDataStore.Should().BeOfType<AccountDataStore>();
        }
    }
}
