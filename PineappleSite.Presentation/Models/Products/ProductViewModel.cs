using PineappleSite.Presentation.Models.Products.Enum;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }

        [Range(1, 10, ErrorMessage = "Выбор в диапазоне от 1 до 10")]
        public int Count { get; set; } = 1;

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}