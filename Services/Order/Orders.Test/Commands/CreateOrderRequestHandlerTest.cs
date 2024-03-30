﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Handlers.Commands;
using Order.Application.Features.Requests.Commands;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Orders.Test.Common;

namespace Orders.Test.Commands
{
    public class CreateOrderRequestHandlerTest : TestCommandsHandler
    {
        [Fact]
        public async Task CreateOrderRequestHandler_Success()
        {
            // Arrange
            var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, CreateValidator);
            var cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    CartHeaderId = 1,
                    UserId = "TestuserId23",
                    CouponCode = "5OFF",
                    Discount = 10,
                    CartTotal = 20,

                    Name = "name",
                    PhoneNumber = "375445678909",
                    Email = "email@gmail.com",
                    Address = "test address",
                },

                CartDetails =
                [
                    new() {
                        CartDetailsId = 1,
                        CartHeaderId = 1,
                        ProductId = 3,
                        Count = 1,
                        Product = new ProductDto(){
                            Id = 1,
                            Name = "name",
                            Description = "descriptiodescriptionn",
                            Price = 25,
                            ProductCategory = Order.Domain.Enum.ProductCategory.Drinks,
                        },
                    },
                ],
            };

            // Act
            var result = await handler.Handle(new CreateOrderRequest
            {
                CartDto = cartDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().Be("Заказ успешно создан");

            var getCreatedOrder = await Context.OrderHeaders.AsNoTracking().FirstOrDefaultAsync(key => key.OrderHeaderId == result.Data.OrderHeaderId);
            getCreatedOrder?.OrderHeaderId.Should().Be(result?.Data?.OrderHeaderId);
            getCreatedOrder?.UserId.Should().Be("TestuserId23");
            getCreatedOrder?.CouponCode.Should().Be("5OFF");
            getCreatedOrder?.Discount.Should().Be(10);
            getCreatedOrder?.OrderTotal.Should().Be(20);
            getCreatedOrder?.Name.Should().Be("name");
            getCreatedOrder?.PhoneNumber.Should().Be("375445678909");
            getCreatedOrder?.Email.Should().Be("email@gmail.com");
            getCreatedOrder?.Address.Should().Be("test address");
            getCreatedOrder?.Email.Should().Be("email@gmail.com");
        }

        [Fact]
        public async Task CreateOrderRequestHandler_FailOrWrongName()
        {
            // Arrange
            var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, CreateValidator);
            var cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    CartHeaderId = 1,
                    UserId = "TestuserId23",
                    CouponCode = "5OFF",
                    Discount = 10,
                    CartTotal = 20,

                    Name = "n",
                    PhoneNumber = "375445678909",
                    Email = "email@gmail.com",
                    Address = "test address",
                    DeliveryDate = DateTime.UtcNow,
                },

                CartDetails =
                [
                    new() {
                        CartDetailsId = 1,
                        CartHeaderId = 1,
                        ProductId = 3,
                        Count = 1,
                        Product = new ProductDto(){
                            Id = 1,
                            Name = "name",
                            Description = "descriptiodescriptionn",
                            Price = 25,
                            ProductCategory = Order.Domain.Enum.ProductCategory.Drinks,
                        },
                    },
                ],
            };

            // Act
            var result = await handler.Handle(new CreateOrderRequest
            {
                CartDto = cartDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Создать заказ невозможно");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task CreateOrderRequestHandler_FailOrWrongEmail()
        {
            // Arrange
            var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, CreateValidator);
            var cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    CartHeaderId = 1,
                    UserId = "TestuserId23",
                    CouponCode = "5OFF",
                    Discount = 10,
                    CartTotal = 20,

                    Name = "name",
                    PhoneNumber = "375445678909",
                    Email = "email",
                    Address = "test address",
                    DeliveryDate = DateTime.UtcNow,
                },

                CartDetails =
                [
                    new() {
                        CartDetailsId = 1,
                        CartHeaderId = 1,
                        ProductId = 3,
                        Count = 1,
                        Product = new ProductDto(){
                            Id = 1,
                            Name = "name",
                            Description = "descriptiodescriptionn",
                            Price = 25,
                            ProductCategory = Order.Domain.Enum.ProductCategory.Drinks,
                        },
                    },
                ],
            };

            // Act
            var result = await handler.Handle(new CreateOrderRequest
            {
                CartDto = cartDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Создать заказ невозможно");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task CreateOrderRequestHandler_FailOrWrongPhoneNumberl()
        {
            // Arrange
            var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, CreateValidator);
            var cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    CartHeaderId = 1,
                    UserId = "TestuserId23",
                    CouponCode = "5OFF",
                    Discount = 10,
                    CartTotal = 20,

                    Name = "name",
                    PhoneNumber = "37",
                    Email = "email@gmail.com",
                    Address = "test address",
                    DeliveryDate = DateTime.UtcNow,
                },

                CartDetails =
                [
                    new() {
                        CartDetailsId = 1,
                        CartHeaderId = 1,
                        ProductId = 3,
                        Count = 1,
                        Product = new ProductDto(){
                            Id = 1,
                            Name = "name",
                            Description = "descriptiodescriptionn",
                            Price = 25,
                            ProductCategory = Order.Domain.Enum.ProductCategory.Drinks,
                        },
                    },
                ],
            };

            // Act
            var result = await handler.Handle(new CreateOrderRequest
            {
                CartDto = cartDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Создать заказ невозможно");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }
    }
}