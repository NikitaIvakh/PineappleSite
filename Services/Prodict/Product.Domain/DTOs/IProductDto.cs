using Product.Domain.Enum;

namespace Product.Domain.DTOs
{
    public interface IProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }
    }
}