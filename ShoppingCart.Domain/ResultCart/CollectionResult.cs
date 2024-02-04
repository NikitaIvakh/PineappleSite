namespace ShoppingCart.Domain.ResultCart
{
    public class CollectionResult<Type> : Result<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}