using ClearBank.DeveloperTest.Services;
using ClearBank.DeveloperTest.Types;
using FluentAssertions;
using Xunit;

namespace ClearBank.DeveloperTest.Tests
{
    public class BacsPaymentValidationShould
    {
        private readonly Account _account;
        private readonly MakePaymentRequest _makePaymentRequest;
        private readonly BacsPaymentValidation _validator;

        public BacsPaymentValidationShould()
        {
            _account = new Account();
            _makePaymentRequest = new MakePaymentRequest();

            _validator = new BacsPaymentValidation();
        }

        [Theory]
        [InlineData(AllowedPaymentSchemes.Chaps)]
        [InlineData(AllowedPaymentSchemes.FasterPayments)]
        public void NotBeValid_GivenNotBacsPaymentSchemes(AllowedPaymentSchemes allowedPaymentSchemes)
        {
            _account.AllowedPaymentSchemes = allowedPaymentSchemes;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeFalse();
        }

        [Fact]
        public void BeValid_GivenBacsPaymentSchemes()
        {
            _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;

            var isValid = _validator.IsPaymentValid(_account, _makePaymentRequest.Amount);

            isValid.Should().BeTrue();
        }
    }
}