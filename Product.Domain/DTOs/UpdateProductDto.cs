using Microsoft.AspNetCore.Http;
using Product.Domain.Enum;

namespace Product.Domain.DTOs
{
    public class UpdateProductDto : IProductDto
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