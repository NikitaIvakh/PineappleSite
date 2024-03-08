using Microsoft.AspNetCore.Http;
using Product.Domain.Enum;

namespace Product.Domain.DTOs
{
    public class CreateProductDto : IProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }

        public IFormFile? Avatar { get; set; }
    }
}