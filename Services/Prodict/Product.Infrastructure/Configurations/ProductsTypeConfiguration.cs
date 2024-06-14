using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Product.Domain.Entities.Producrs;
using Product.Domain.Enum;

namespace Product.Infrastructure.Configurations;

public sealed class ProductsTypeConfiguration : IEntityTypeConfiguration<ProductEntity>
{
    public void Configure(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasKey(key => key.Id);
        builder.Property(key => key.ProductCategory).HasConversion<string>();
        builder.Property(key => key.Price).HasColumnType("numeric(10, 2)");

        // SeedData(builder);
    }

    private static void SeedData(EntityTypeBuilder<ProductEntity> builder)
    {
        builder.HasData(
            new ProductEntity
            {
                Id = 1,
                Name = "Борщ",
                Description = "Самый вкусный суп.",
                ProductCategory = ProductCategory.Soups,
                Price = 10,
                ImageUrl = "https://placehold.co/600x400",
            },
            new ProductEntity
            {
                Id = 2,
                Name = "Сырные шарики",
                Description = "Сырные шарики с крабовыми палочками.",
                ProductCategory = ProductCategory.Snacks,
                Price = 15,
                ImageUrl = "https://placehold.co/601x401",
            },
            new ProductEntity
            {
                Id = 3,
                Name = "Кока-кола",
                Description = "Газированный напиток.",
                ProductCategory = ProductCategory.Drinks,
                Price = 5,
                ImageUrl = "https://placehold.co/602x402",
            });
    }
}