using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public int OrderDetailsId { get; set; }

        public int OrderHeaderId { get; set; }

        public ProductViewModel? Product { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }
    }
}