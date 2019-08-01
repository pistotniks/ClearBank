using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceShould
    {
        private readonly Mock<IAccountService> _accountService;
        private readonly Mock<IPaymentsValidationService> _paymentsValidationService;
        private readonly PaymentService _paymentService;

        public PaymentServiceShould()
        {
            _accountService = new Mock<IAccountService>();
            _paymentsValidationService = new Mock<IPaymentsValidationService>();
            _paymentService = new PaymentService(_accountService.Object, _paymentsValidationService.Object);
        }

        [Fact]
        public void MakePaymentWithSuccess_GivenValidPayment()
        {
            _accountService.Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(new Account());
            _paymentsValidationService.Setup(service => service.ValidatePayment(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<PaymentScheme>()))
                .Returns(true);

            var result = _paymentService.MakePayment(new MakePaymentRequest());

            result.Success.Should().BeTrue();
        }

       [Fact]
        public void UpdateAccount_GivenValidPayment()
        {
            _accountService.Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(new Account());
            _paymentsValidationService.Setup(service => service.ValidatePayment(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<PaymentScheme>()))
                .Returns(true);

            _paymentService.MakePayment(new MakePaymentRequest());

            _accountService .Verify(service => service.UpdateAccount(
                    It.IsAny<Account>(), 
                    It.IsAny<decimal>()), Times.Once);
        }

        [Fact]
        public void FailMakingPayment_GivenInvalidPayment()
        {
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(new Account());
            _paymentsValidationService.Setup(service => service.ValidatePayment(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<PaymentScheme>()))
                .Returns(false);

            var result = _paymentService.MakePayment(new MakePaymentRequest());

            result.Success.Should().BeFalse();
        }

        [Fact]
        public void NotUpdateAccount_GivenInvalidPayment()
        {
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(new Account());
            _paymentsValidationService.Setup(service => service.ValidatePayment(It.IsAny<Account>(), It.IsAny<decimal>(), It.IsAny<PaymentScheme>()))
                .Returns(false);

            _paymentService.MakePayment(new MakePaymentRequest());

            _accountService.Verify(service => service.UpdateAccount(
                It.IsAny<Account>(),
                It.IsAny<decimal>()), Times.Never);
        }
    }
}
