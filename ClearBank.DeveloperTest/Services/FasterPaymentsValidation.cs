using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class FasterPaymentsValidation : IPaymentValidator
    {
        public virtual bool IsPaymentValid(Account account, decimal requestedAmount)
        {
            if (account == null) return false;

            if (!account.AllowedPaymentSchemes.HasFlag(AllowedPaymentSchemes.FasterPayments))
            {
                return false;
            }

            return account.Balance >= requestedAmount;
        }
    }
}