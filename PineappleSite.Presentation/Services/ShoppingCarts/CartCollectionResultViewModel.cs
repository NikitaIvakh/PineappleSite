namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class CartCollectionResultViewModel<TEntity> : CartResultViewModel<IReadOnlyCollection<TEntity>>
    {
        public int Count { get; set; }
    }
}