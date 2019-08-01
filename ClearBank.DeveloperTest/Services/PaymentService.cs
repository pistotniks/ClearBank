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

            if (_paymentsValidationService.ValidatePayment(account, request.Amount, request.PaymentScheme))
            {
                _accountService.UpdateAccount(account, request.Amount);
                return MakePaymentResult.OK();
            }

            return MakePaymentResult.Fail();
        }
    }
}
