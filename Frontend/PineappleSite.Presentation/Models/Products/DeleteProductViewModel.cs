namespace PineappleSite.Presentation.Models.Products;

public sealed class DeleteProductViewModel(int id)
{
    public int Id { get; set; } = id;
}