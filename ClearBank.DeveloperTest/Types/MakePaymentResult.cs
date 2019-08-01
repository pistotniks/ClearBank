namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        public bool Success { get; private set; }

        public MakePaymentResult()
        {
            Success = true;
        }

        public static MakePaymentResult OK()
        {
            return new MakePaymentResult();
        }

        public static MakePaymentResult Fail()
        {
            return new MakePaymentResult(){ Success = false};
        }
    }
}
