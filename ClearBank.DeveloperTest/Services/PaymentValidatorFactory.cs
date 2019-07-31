using System.Collections.Generic;
using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentValidatorFactory : IPaymentValidatorFactory
    {
        private readonly IDictionary<PaymentScheme, IPaymentValidator> _paymentValidators =
            new Dictionary<PaymentScheme, IPaymentValidator>
            {
                { PaymentScheme.Bacs, new BacsPaymentValidation() },
                { PaymentScheme.FasterPayments, new FasterPaymentsValidation() },
                { PaymentScheme.Chaps, new ChapsPaymentValidation()}
            };

        public PaymentValidatorFactory()
        {
        }

        public PaymentValidatorFactory(IDictionary<PaymentScheme, IPaymentValidator> paymentValidators)
        {
            _paymentValidators = paymentValidators;
        }

        public IPaymentValidator CreateValidator(PaymentScheme paymentScheme)
        {
            return _paymentValidators[paymentScheme];
        }
    }
}