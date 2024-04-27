using PineappleSite.Presentation.Models.Products;

namespace PineappleSite.Presentation.Models.Orders;

public sealed class OrderDetailsViewModel
{
    public int OrderDetailsId { get; init; }

    public int OrderHeaderId { get; init; }

    public ProductViewModel? Product { get; init; }

    public int ProductId { get; init; }

    public int Count { get; init; }

    public string? ProductName { get; init; }

    public double Price { get; init; }
}