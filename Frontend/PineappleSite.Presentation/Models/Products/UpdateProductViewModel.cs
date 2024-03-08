using PineappleSite.Presentation.Models.Products.Enum;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Products
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }

        [MaxLength(30, ErrorMessage = "Строка не должна превышать 30 символов")]
        [MinLength(3, ErrorMessage = "Строка должна быть более 3 символов")]
        public string Name { get; set; }

        [MaxLength(500, ErrorMessage = "Строка не должна превышать 500 символов")]
        [MinLength(10, ErrorMessage = "Строка должна быть более 10 символов")]
        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        [Range(5, 1000, ErrorMessage = "Стоимость товара доложна быть более 5, но не более 1000")]
        public double Price { get; set; }

        public IFormFile? Avatar { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}