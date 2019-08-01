using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;

        public PaymentService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public MakePaymentResult MakePayment(MakePaymentRequest request)
        {
            var account = _accountService.GetAccount(request.DebtorAccountNumber);
            var result = ValidatePayment(request, account);

            if (result.Success)
            {
                _accountService.UpdateAccount(account, request.Amount);
            }

            return result;
        }

        private static MakePaymentResult ValidatePayment(MakePaymentRequest request, Account account)
        {
            var result = new MakePaymentResult();
            result.Success = new PaymentsValidationService(new PaymentValidatorFactory()).ValidatePayment(account, request.Amount, request.PaymentScheme);
            return result;
        }
    }
}
