﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PineappleSite.Presentation.Extecsions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType().GetMember(value.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()?.GetName() ?? value.ToString();
        }
    }
}