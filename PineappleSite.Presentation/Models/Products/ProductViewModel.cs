using PineappleSite.Presentation.Models.Products.Enum;

namespace PineappleSite.Presentation.Models.Products
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }
    }
}