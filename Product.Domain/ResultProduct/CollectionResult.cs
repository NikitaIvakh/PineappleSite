using System.Collections.Generic;

namespace Product.Domain.ResultProduct
{
    public class CollectionResult<TEntity> : Result<IReadOnlyCollection<TEntity>> where TEntity : class
    {
        public int Count { get; set; }
    }
}