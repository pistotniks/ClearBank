using ClearBank.DeveloperTest.Types;

namespace ClearBank.DeveloperTest.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly IPaymentsValidationService _paymentsValidationService;

        public PaymentService(IAccountService accountService, IPaymentsValidationService paymentsValidationService)
        {
            _accountService = accountService;
            _paymentsValidationService = paymentsValidationService;
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

        private MakePaymentResult ValidatePayment(MakePaymentRequest request, Account account)
        {
            return new MakePaymentResult
            {
                Success = _paymentsValidationService.ValidatePayment(account, request.Amount, request.PaymentScheme)
            };
        }
    }
}
