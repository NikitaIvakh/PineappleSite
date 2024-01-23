namespace ShoppingCart.Application.Response
{
    public class ShoppingCartAPIResponse
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = true;

        public List<string> ValidationErrors { get; set; }
    }
}