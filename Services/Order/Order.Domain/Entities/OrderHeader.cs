﻿namespace Order.Domain.Entities
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }

        public string? UserId { get; set; }

        public string? CouponCode { get; set; }

        public double Discount { get; set; }

        public double OrderTotal { get; set; }


        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public DateTime? DeliveryDate { get; set; }


        public DateTime OrderTime { get; set; }

        public string? Status { get; set; }

        public string? PaymentIntentId { get; set; }

        public string? StripeSessionId { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }
    }
}