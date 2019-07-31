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
        private readonly PaymentService _paymentService;

        public PaymentServiceShould()
        {
            _accountService = new Mock<IAccountService>();
            _paymentService = new PaymentService(_accountService.Object);
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps, 3, 1, AccountStatus.Live)]
        public void MakePaymentAndUpdateAccount_GivenValidAccountAndAnyPaymentSchemeAndValidRequest(
            AllowedPaymentSchemes paymentSchemes, PaymentScheme requestedPaymentScheme, decimal accountBalance, decimal requestedAmount, AccountStatus accountStatus)
        {
            var validAccount = new Account
            {
                AllowedPaymentSchemes = paymentSchemes,
                Balance = accountBalance,
                Status = accountStatus
            };
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);

            var paymentRequest = new MakePaymentRequest {Amount = requestedAmount, PaymentScheme = requestedPaymentScheme};

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeTrue();
            _accountService .Verify(service => service.UpdateAccount(
                    It.IsAny<Account>(), 
                    It.IsAny<decimal>()), Times.Once);
        }

        [Theory]
        [InlineData(PaymentScheme.Bacs, 1)]
        [InlineData(PaymentScheme.FasterPayments, 1)]
        [InlineData(PaymentScheme.Chaps, 1)]
        public void NotMakePaymentAndNotUpdateAccount_GivenNoAccount(
            PaymentScheme requestedPaymentScheme, decimal requestedAmount)
        {
            Account noAccount = null;
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(noAccount);
            var paymentRequest = new MakePaymentRequest {Amount = requestedAmount, PaymentScheme = requestedPaymentScheme};

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            _accountService.Verify(service => service.UpdateAccount(
                It.IsAny<Account>(),
                It.IsAny<decimal>()), Times.Never);
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.Bacs, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.FasterPayments, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Chaps, 3, 1, AccountStatus.Live)]
        public void NotMakePaymentAndNotUpdateAccount_GivenAccountDoesNotAllowThePaymentSchemeProvided(
            AllowedPaymentSchemes paymentSchemes, PaymentScheme requestedPaymentScheme, decimal accountBalance, decimal requestedAmount, AccountStatus accountStatus)
        {
            var validAccount = new Account
            {
                AllowedPaymentSchemes = paymentSchemes,
                Balance = accountBalance,
                Status = accountStatus
            };
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = requestedAmount, PaymentScheme = requestedPaymentScheme };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            _accountService.Verify(service => service.UpdateAccount(
                It.IsAny<Account>(),
                It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void NotMakePaymentAndNotUpdateAccount_GivenFasterPaymentsRequested_And_BalanceIsNotSufficient()
        {
            const decimal accountBalance = 1;
            const decimal tooHighRequestedAmount = 2;
            var validAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments,
                Balance = accountBalance,
                Status = AccountStatus.Live
            };
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = tooHighRequestedAmount, PaymentScheme = PaymentScheme.FasterPayments };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            _accountService.Verify(service => service.UpdateAccount(
                It.IsAny<Account>(),
                It.IsAny<decimal>()), Times.Never);
        }

        [Fact]
        public void NotMakePaymentAndNotUpdateAccount_GivenChapsPaymentsRequested_And_AccountStatusIsNotLive()
        {
            var validAccount = new Account
            {
                AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps,
                Balance = 1,
                Status = AccountStatus.Disabled
            };
            _accountService
                .Setup(service => service.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = 1, PaymentScheme = PaymentScheme.Chaps };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            _accountService.Verify(service => service.UpdateAccount(
                It.IsAny<Account>(),
                It.IsAny<decimal>()), Times.Never);
        }

    }
}
