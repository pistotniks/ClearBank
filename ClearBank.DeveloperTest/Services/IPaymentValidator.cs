using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValidator
    {
        bool IsPaymentValid(Account account, decimal requestedAmount);
    }
}