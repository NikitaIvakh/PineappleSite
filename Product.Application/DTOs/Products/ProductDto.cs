﻿using Product.Core.Entities.Enum;

namespace Product.Application.DTOs.Products
{
    public class ProductDto : IProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public double Price { get; set; }
    }
}