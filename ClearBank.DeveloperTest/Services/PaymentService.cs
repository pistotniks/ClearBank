using ClearBank.DeveloperTest.Data;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private IAccountDataStore _accountDataStore;
        private readonly IAppConfiguration _configuration;

        public PaymentService(IAccountDataStore accountDataStore, IAppConfiguration configuration)
        {
            _accountDataStore = accountDataStore;
            _configuration = configuration;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            string dataStoreType = _configuration.DataStoreType;

            if (dataStoreType == "Backup")
            {
                _accountDataStore = new BackupAccountDataStore();
            }
            else
            {
                _accountDataStore = new AccountDataStore();

            }
            Account account = _accountDataStore.GetAccount(request.DebtorAccountNumber);
            var result = ValidatePayment(request, account);

            if (result.Success)
            {
                account.Balance -= request.Amount;
                _accountDataStore.UpdateAccount(account);
            }

            return result;
        }

        private static MakePaymentResult ValidatePayment(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();

            switch (request.PaymentScheme)
            {
                case PaymentScheme.Bacs:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Bacs))
                    {
                        result.Success = false;
                    }

                    break;

                case PaymentScheme.FasterPayments:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
                    {
                        result.Success = false;
                    }
                    else if (account.Balance < request.Amount)
                    {
                        result.Success = false;
                    }

                    break;

                case PaymentScheme.Chaps:
                    if (account == null)
                    {
                        result.Success = false;
                    }
                    else if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
                    {
                        result.Success = false;
                    }
                    else if (account.Status != AccountStatus.Live)
                    {
                        result.Success = false;
                    }

                    break;
            }

            return result;
        }
    }
}
