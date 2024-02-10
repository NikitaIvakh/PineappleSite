﻿using System.ComponentModel.DataAnnotations;

namespace PineappleSite.Presentation.Models.Orders
{
    public class OrderHeaderViewModel
    {
        public int OrderHeaderId { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double OrderTotal { get; set; }

        [Required(ErrorMessage = "Поле с именем не может быть пустым")]
        [MaxLength(45, ErrorMessage = "Поле не может превышать 45 символов")]
        [MinLength(2, ErrorMessage = "Имя должно быть более 2 символов")]
        public string? Name { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "Ведите действительный адрес электронной почты")]
        [EmailAddress(ErrorMessage = "Почтовый адрес не может быть пустым")]
        [Required(ErrorMessage = "Поле с почтовым адресом не может быть пустым")]
        [MaxLength(45, ErrorMessage = "Почтовый адрес не должен превышать 45 символов")]
        [MinLength(2, ErrorMessage = "Почтовый адрес должен быть более 2 символов")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле с номером телефона не может быть пустым")]
        [MaxLength(12, ErrorMessage = "Номер телефона не должен превышать 12 символов")]
        [MinLength(7, ErrorMessage = "Номер телефона должен быть более 7 символов")]
        public string? PhoneNumber { get; set; }

        public DateTime OrderTime { get; set; }

        public string? Status { get; set; }

        public string? PaymentIntentId { get; set; }

        public string? StripeSessionId { get; set; }

        public IEnumerable<OrderDetailsViewModel> OrderDetails { get; set; }
    }
}