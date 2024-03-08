namespace Coupon.Domain.ResultCoupon
{
    public class CollectionResult<Type> : Result<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}