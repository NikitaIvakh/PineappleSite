﻿using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Coupons
{
    public class CreateCouponViewModel
    {
        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [MaxLength(20, ErrorMessage = "Строка не должна превышать 20 символов")]
        [MinLength(3, ErrorMessage = "Строка должна превышать 3 символа")]
        public string CouponCode { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(2, 101, ErrorMessage = "Сумма скидки должна превышать 2 единицы, но не превышать 101 единицу")]
        public double DiscountAmount { get; set; }

        [Required(ErrorMessage = "Поле обязательно для заполнения")]
        [Range(2, 101, ErrorMessage = "Стоимость товара должна превышать 2 единицы, но не превышать 101 единицу")]
        public double MinAmount { get; set; }
    }
}