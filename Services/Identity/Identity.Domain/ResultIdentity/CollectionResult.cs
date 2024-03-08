namespace Identity.Domain.ResultIdentity
{
    public class CollectionResult<Type> : Result<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}