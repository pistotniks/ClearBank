using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentsValidationService : IPaymentsValidationService
    {
        private readonly IPaymentValidatorFactory _factory;

        public PaymentsValidationService(IPaymentValidatorFactory factory)
        {
            _factory = factory;
        }

        public bool ValidatePayment(Account account, decimal requestedAmount, PaymentScheme paymentScheme)
        {
            var validator = _factory.CreateValidator(paymentScheme);
            return validator.IsPaymentValid(account, requestedAmount);
        }
    }
}