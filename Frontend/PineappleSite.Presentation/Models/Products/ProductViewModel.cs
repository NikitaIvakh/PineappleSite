using PineappleSite.Presentation.Models.Products.Enum;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Products;

public sealed class ProductViewModel
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public required string Description { get; init; }

    public ProductCategory ProductCategory { get; init; }

    public double Price { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполения")]
    [Range(1, 10, ErrorMessage = "Выбор в диапазоне от 1 до 10")]
    public int Count { get; init; } = 1;

    public string? ImageUrl { get; init; }

    public string? ImageLocalPath { get; init; }
}