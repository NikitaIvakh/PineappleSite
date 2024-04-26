namespace PineappleSite.Presentation.Services.ShoppingCarts;

public class CartCollectionResult<T> : CartResult<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}