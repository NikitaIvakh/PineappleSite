namespace Coupon.Domain.ResultCoupon
{
    public class CollectionResult<T> : Result<IReadOnlyCollection<T>>
    {
        public int Count { get; set; }
    }
}