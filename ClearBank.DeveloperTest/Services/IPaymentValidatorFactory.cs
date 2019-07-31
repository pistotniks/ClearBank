using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public interface IPaymentValidatorFactory
    {
        IPaymentValidator CreateValidator(PaymentScheme paymentScheme);
    }
}