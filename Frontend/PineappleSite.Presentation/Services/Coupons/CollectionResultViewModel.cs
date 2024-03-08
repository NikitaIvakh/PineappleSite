namespace PineappleSite.Presentation.Services.Coupons
{
    public class CollectionResultViewModel<Type> : ResultViewModel<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}