namespace PineappleSite.Presentation.Services.Coupons;

public class CollectionResultViewModel<T> : ResultViewModel<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}