using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Products.Enum
{
    public enum ProductCategory
    {
        [Display(Name = "Суп")]
        Soups = 1,

        [Display(Name = "Закуска")]
        Snacks = 2,

        [Display(Name = "Напиток")]
        Drinks = 3,
    }
}