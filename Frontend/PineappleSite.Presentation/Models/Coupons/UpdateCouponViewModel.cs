﻿using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Coupons;

public sealed class UpdateCouponViewModel
{
    public string? CouponId { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [MaxLength(20, ErrorMessage = "Строка не должна превышать 20 символов")]
    [MinLength(3, ErrorMessage = "Строка должна превышать 3 символа")]
    public required string CouponCode { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Range(2, 101, ErrorMessage = "Сумма скидки должна превышать 2 единицы, но не превышать 101 единицу")]
    public required double DiscountAmount { get; init; }

    [Required(ErrorMessage = "Поле обязательно для заполнения")]
    [Range(2, 101, ErrorMessage = "Стоимость товара должна превышать 2 единицы, но не превышать 101 единицу")]
    public required double MinAmount { get; init; }
}