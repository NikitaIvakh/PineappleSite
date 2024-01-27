namespace Coupon.Domain.ResultCoupon
{
    public class Result
    {
        public bool IsSuccess => ErrorMessage == null;

        public string ErrorMessage { get; set; }

        public int? ErrorCode { get; set; }
    }

    public class Result<Type> : Result
    {
        public Result(string errorMessage, int errorCode, Type data)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            Data = data;
        }

        public Result(string errorMessage, int? errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        public Result(Type data)
        {
            Data = data;
        }

        public Result()
        {

        }

        public Type Data { get; set; }
    }
}