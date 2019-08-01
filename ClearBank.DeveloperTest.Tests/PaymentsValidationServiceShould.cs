using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentsValidationServiceShould
    {
        private readonly PaymentsValidationService _validationService;
        private readonly Mock<IPaymentValidatorFactory> _factory;

        public PaymentsValidationServiceShould()
        {
            _factory = new Mock<IPaymentValidatorFactory>();

            _validationService = new PaymentsValidationService(_factory.Object);
        }

        [Fact]
        public void ValidatePayment_GivenNoPaymentSchemeAndUsingFasterPaymentsValidation()
        {
            var validation = new Mock<IPaymentValidator>();
            var makePaymentRequest = new MakePaymentRequest();
            _factory.Setup(x => x.CreateValidator(It.Is<PaymentScheme>(y => y.Equals(PaymentScheme.FasterPayments)))).Returns(validation.Object);

            _validationService.ValidatePayment(new Account(), makePaymentRequest.Amount, makePaymentRequest.PaymentScheme);

            validation.Verify(x => x.IsPaymentValid(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void ValidatePayment_GivenFasterPaymentsPaymentSchemeAndUsingFasterPaymentsValidation()
        {
            var validation = new Mock<IPaymentValidator>();
            _factory.Setup(x => x.CreateValidator(It.Is<PaymentScheme>(y => y.Equals(PaymentScheme.FasterPayments)))).Returns(validation.Object);
            var makePaymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.FasterPayments };

            _validationService.ValidatePayment(new Account(), makePaymentRequest.Amount, makePaymentRequest.PaymentScheme);

            validation.Verify(x => x.IsPaymentValid(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void ValidatePayment_GivenChapsPaymentsPaymentSchemeAndUsingChapsPaymentsValidation()
        {
            var validation = new Mock<IPaymentValidator>();
            _factory.Setup(x => x.CreateValidator(It.Is<PaymentScheme>(y => y.Equals(PaymentScheme.Chaps)))).Returns(validation.Object);
            var makePaymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Chaps };

            _validationService.ValidatePayment(new Account(), makePaymentRequest.Amount, makePaymentRequest.PaymentScheme);

            validation.Verify(x => x.IsPaymentValid(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void ValidatePayment_GivenBacsPaymentsPaymentSchemeAndUsingBacsPaymentsValidation()
        {
            var validation = new Mock<IPaymentValidator>();
            _factory.Setup(x => x.CreateValidator(It.Is<PaymentScheme>(y => y.Equals(PaymentScheme.Bacs)))).Returns(validation.Object);
            var makePaymentRequest = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

            _validationService.ValidatePayment(new Account(), makePaymentRequest.Amount, makePaymentRequest.PaymentScheme);

            validation.Verify(x => x.IsPaymentValid(It.IsAny<Account>(), It.IsAny<decimal>()), Times.Once);
        }
    }
}