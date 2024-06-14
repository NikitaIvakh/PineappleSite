using PineappleSite.Presentation.Models.Products.Enum;
using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Products;

public sealed class CreateProductViewModel
{
    [Required(ErrorMessage = "Поле обязательно к заполнению")]
    [MaxLength(100, ErrorMessage = "Строка не должна превышать 100 символов")]
    [MinLength(3, ErrorMessage = "Строка должна быть более 3 символов")]
    public required string Name { get; init; }

    [Required(ErrorMessage = "Поле обязательно к заполнению")]
    [MaxLength(500, ErrorMessage = "Строка не должна превышать 500 символов")]
    [MinLength(10, ErrorMessage = "Строка должна быть более 10 символов")]
    public required string Description { get; init; }

    [Required(ErrorMessage = "Поле обязательно к заполнению")]
    public ProductCategory ProductCategory { get; init; }

    [Required(ErrorMessage = "Поле обязательно к заполнению")]
    [Range(5, 1000, ErrorMessage = "Стоимость товара доложна быть более 5, но не более 1000")]
    public double Price { get; init; }

    public IFormFile? Avatar { get; init; }

    public string? ImageUrl { get; init; }

    public string? ImageLocalPath { get; init; }
}