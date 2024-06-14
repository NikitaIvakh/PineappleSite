﻿// <auto-generated />
using Coupon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Coupon.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240614165532_UpdateTyped")]
    partial class UpdateTyped
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Coupon.Domain.Entities.CouponEntity", b =>
                {
                    b.Property<string>("CouponId")
                        .HasColumnType("text");

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("DiscountAmount")
                        .HasColumnType("numeric(10, 2)");

                    b.Property<double>("MinAmount")
                        .HasColumnType("numeric(10, 2)");

                    b.HasKey("CouponId");

                    b.ToTable("Coupons", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
