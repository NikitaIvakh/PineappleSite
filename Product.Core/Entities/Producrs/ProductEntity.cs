using Product.Core.Entities.Enum;

namespace Product.Core.Entities.Producrs
{
    public class ProductEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }
    }
}