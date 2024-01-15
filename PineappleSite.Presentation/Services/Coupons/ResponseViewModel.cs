namespace PineappleSite.Presentation.Services.Coupons
{
    public class ResponseViewModel
    {
        public int Id { get; set; }

        public string Message { get; set; } = string.Empty;

        public bool IsSuccess { get; set; }

        public string ValidationErrors { get; set; }
    }
}