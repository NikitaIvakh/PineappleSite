using PineappleSite.Presentation.Models.Products.Enum;
using PineappleSite.Presentation.Services.Identities;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PineappleSite.Presentation.Extecsions;

public static class EnumExtensions
{
    public static string GetDisplayName(this Enum value)
    {
        return value.GetType().GetMember(value.ToString())
            .First()
            .GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
    }

    public static string GetDisplayName(this ProductCategory category)
    {
        var displayAttribute = typeof(ProductCategory)
            .GetField(category.ToString())!
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? category.ToString();
    }

    public static string GetDisplayName(this UserRoles userRoles)
    {
        var displayAttribute = typeof(UserRoles)
            .GetField(userRoles.ToString())!
            .GetCustomAttributes(typeof(DisplayAttribute), false)
            .FirstOrDefault() as DisplayAttribute;

        return displayAttribute?.Name ?? userRoles.ToString();
    }
}