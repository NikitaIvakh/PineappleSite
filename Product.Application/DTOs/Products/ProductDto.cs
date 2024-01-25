using Product.Core.Entities.Enum;
using Microsoft.AspNetCore.Http;

namespace Product.Application.DTOs.Products
{
    public class ProductDto : IProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}