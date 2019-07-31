using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Moq;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class PaymentServiceShould
    {
        private readonly Mock<IAccountDataStore> _accountDataStore;
        private readonly PaymentService _paymentService;

        public PaymentServiceShould()
        {
            _accountDataStore = new Mock<IAccountDataStore>();
            _paymentService = new PaymentService(_accountDataStore.Object);
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs, PaymentScheme.Bacs, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.FasterPayments, PaymentScheme.FasterPayments, 3, 1, AccountStatus.Disabled)]
        [InlineData(AllowedPaymentSchemes.Chaps, PaymentScheme.Chaps, 3, 1, AccountStatus.Live)]
        public void MakePaymentAndUpdateAccountWithDeductedBalance_GivenValidAccountAndAnyPaymentSchemeAndValidRequest(
            AllowedPaymentSchemes paymentSchemes, PaymentScheme requestedPaymentScheme, decimal accountBalance, decimal requestedAmount, AccountStatus accountStatus)
        {
            var validAccount = new Account
            {
                AllowedPaymentSchemes = paymentSchemes,
                Balance = accountBalance,
                Status = accountStatus
            };
            _accountDataStore
                .Setup(validator => validator.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest {Amount = requestedAmount, PaymentScheme = requestedPaymentScheme};

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeTrue();
            validAccount.Balance.Should().Be(accountBalance - requestedAmount);
        }

        [Theory]
        [InlineData(PaymentScheme.Bacs, 1)]
        [InlineData(PaymentScheme.FasterPayments, 1)]
        [InlineData(PaymentScheme.Chaps, 1)]
        public void NotMakePaymentAndNotUpdateAccount_GivenNoAccount(
            PaymentScheme requestedPaymentScheme, decimal requestedAmount)
        {
            Account noAccount = null;
            _accountDataStore
                .Setup(validator => validator.GetAccount(It.IsAny<string>()))
                .Returns(noAccount);
            var paymentRequest = new MakePaymentRequest {Amount = requestedAmount, PaymentScheme = requestedPaymentScheme};

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
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
            _accountDataStore
                .Setup(validator => validator.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = requestedAmount, PaymentScheme = requestedPaymentScheme };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            validAccount.Balance.Should().Be(accountBalance);
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
            _accountDataStore
                .Setup(validator => validator.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = tooHighRequestedAmount, PaymentScheme = PaymentScheme.FasterPayments };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            validAccount.Balance.Should().Be(accountBalance);
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
            _accountDataStore
                .Setup(validator => validator.GetAccount(It.IsAny<string>()))
                .Returns(validAccount);
            var paymentRequest = new MakePaymentRequest { Amount = 1, PaymentScheme = PaymentScheme.Chaps };

            var result = _paymentService.MakePayment(paymentRequest);

            result.Success.Should().BeFalse();
            validAccount.Balance.Should().Be(1);
        }

    }
}
