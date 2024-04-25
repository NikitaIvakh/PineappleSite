using System.ComponentModel.DataAnnotations;
using PineappleSite.Presentation.Models.Products.Enum;

namespace PineappleSite.Presentation.Models.Products;

public sealed record GetProductViewModel(
    int Id,
    string Name,
    string Description,
    ProductCategory ProductCategory,
    double Price,
    string? ImageUrl,
    string? ImageLocalPath,
    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Range(1, 10, ErrorMessage = "Выбор в диапазоне от 1 до 10")]
    int Count);