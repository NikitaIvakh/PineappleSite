﻿using Order.Domain.Enum;

namespace Order.Domain.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        public string? ImageLocalPath { get; set; }
    }
}