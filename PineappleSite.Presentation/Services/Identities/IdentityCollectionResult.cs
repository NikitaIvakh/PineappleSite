namespace PineappleSite.Presentation.Services.Identities
{
    public class IdentityCollectionResult<Type> : IdentityResult<IReadOnlyCollection<Type>>
    {
        public int Count { get; set; }
    }
}