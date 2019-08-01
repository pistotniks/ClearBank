using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentsValidationService
    {
        bool ValidatePayment(Account account, decimal requestedAmount, PaymentScheme paymentScheme);
    }
}