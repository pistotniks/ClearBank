using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class FasterPaymentsValidationShould
    {
        private readonly FasterPaymentsValidation _validator;
        private readonly Account _account;
        private readonly MakePaymentRequest _makePaymentRequest;

        public FasterPaymentsValidationShould()
        {
            _account = new Account();
            _makePaymentRequest = new MakePaymentRequest();

            _validator = new FasterPaymentsValidation();
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Bacs)]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        public void NotBeValid_GivenNonPaymentFasterPaymentsSchemes(AllowedPaymentSchemes paymentSchemes)
        {
            _account.AllowedPaymentSchemes = paymentSchemes;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void BeValid_GivenSufficientAccountBalance()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            _account.Balance = 10;
            _makePaymentRequest.Amount = 9;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeTrue();
        }

        [Fact]
        public void NotBeValid_GivenSufficientAccountBalancee()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
            _account.Balance = 10;
            _makePaymentRequest.Amount = 11;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeFalse();
        }
    }
}