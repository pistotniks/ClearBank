using System;
using System.Collections.Generic;
using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentValidatorFactoryShould
    {
        private readonly PaymentValidatorFactory _factory;

        public PaymentValidatorFactoryShould()
        {
            _factory = new PaymentValidatorFactory( new Dictionary<PaymentScheme, IPaymentValidator>
                {
                    { PaymentScheme.Bacs, new BacsPaymentValidation() },
                    { PaymentScheme.FasterPayments, new FasterPaymentsValidation() },
                    { PaymentScheme.Chaps, new ChapsPaymentValidation() }
                }
            );
        }

        [Theory]
        [InlineData(PaymentScheme.Chaps, typeof(ChapsPaymentValidation))]
        [InlineData(PaymentScheme.Bacs, typeof(BacsPaymentValidation))]
        [InlineData(PaymentScheme.FasterPayments, typeof(FasterPaymentsValidation))]
        public void CreatePaymentValidation_GivenPaymentScheme(PaymentScheme paymentScheme, Type type)
        {
            var paymentValidator = _factory.CreateValidator(paymentScheme);

            paymentValidator.Should().BeOfType(type);
        }
    }
}