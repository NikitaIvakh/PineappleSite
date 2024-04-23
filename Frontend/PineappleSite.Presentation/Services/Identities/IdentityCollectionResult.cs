namespace PineappleSite.Presentation.Services.Identities;

public sealed class IdentityCollectionResult<T> : IdentityResult<IReadOnlyCollection<T>>
{
    public int Count { get; set; }
}