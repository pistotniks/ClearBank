using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class ChapsPaymentValidation : IPaymentValidator
    {
        public virtual bool IsPaymentValid(Account account, decimal requestedAmount)
        {
            if (account == null) return false;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.Chaps))
            {
                return false;
            }

            return account.Status == AccountStatus.Live;
        }
    }
}