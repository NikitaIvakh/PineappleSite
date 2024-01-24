namespace PineappleSite.Presentation.Services.ShoppingCarts
{
    public class ShoppingCartResponseViewModel
    {
        public int Id { get; set; }

        public object? Data { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = true;

        public string ValidationErrors { get; set; }
    }
}