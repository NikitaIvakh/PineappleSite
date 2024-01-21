namespace PineappleSite.Presentation.Services.Products
{
    public class ProductAPIViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string ValidationErrors { get; set; }
    }
}