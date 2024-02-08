﻿using Order.Domain.Entities;

namespace Order.Domain.DTOs
{
    public class OrderDetailsDto
    {
        public int OrderDetailsId { get; set; }

        public OrderHeader? OrderHeader { get; set; }

        public int OrderHeaderId { get; set; }

        public ProductDto? Product { get; set; }

        public int ProductId { get; set; }

        public int Count { get; set; }

        public string ProductName { get; set; }

        public double Price { get; set; }
    }
}