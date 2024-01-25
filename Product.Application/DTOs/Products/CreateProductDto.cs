using Microsoft.AspNetCore.Http;
using Product.Core.Entities.Enum;

namespace Product.Application.DTOs.Products
{
    public class CreateProductDto : IProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}