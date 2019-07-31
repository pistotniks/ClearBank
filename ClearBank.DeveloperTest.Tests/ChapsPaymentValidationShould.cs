using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class ChapsPaymentValidationShould
    {
        private readonly Account _account;
        private readonly MakePaymentRequest _makePaymentRequest;
        private readonly ChapsPaymentValidation _validator;

        public ChapsPaymentValidationShould()
        {
            _account = new Account();
            _makePaymentRequest = new MakePaymentRequest();

            _validator = new ChapsPaymentValidation();
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        public void NotBeValid_GivenNonChapsPaymentSchemes(AllowedPaymentSchemes paymentSchemes)
        {
            _account.AllowedPaymentSchemes = paymentSchemes;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeFalse();
        }

        [Theory]
        [InlineData(AccountStatus.InboundPaymentsOnly)]
        [InlineData(AccountStatus.Disabled)]
        public void NotBeValid_GivenAccountStatusNotLive(AccountStatus accountStatus)
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _account.Status = accountStatus;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void BeValid_GivenAccountStatusLive()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
            _account.Status = AccountStatus.Live;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeTrue();
        }
    }
}