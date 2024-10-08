﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Infrastructure.Configuration;

public sealed class CartDetailsTypeConfiguration : IEntityTypeConfiguration<CartDetails>
{
    public void Configure(EntityTypeBuilder<CartDetails> builder)
    {
        builder.HasKey(key => key.CartDetailsId);
        builder.HasOne(key => key.CartHeader).WithMany().HasForeignKey(key => key.CartHeaderId);
        builder.Property(key => key.Count).HasColumnType("numeric(10, 2)");

        builder.Ignore(key => key.Product);
    }
}