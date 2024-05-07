using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Handlers.Commands;
using Order.Application.Features.Requests.Commands;
using Order.Domain.DTOs;
using Orders.Test.Common;

namespace Orders.Test.Commands;


public sealed class CreateOrderRequestHandlerTest : TestCommandsHandler
{
    [Fact]
    public async Task CreateOrderRequestHandler_Success()
    {
        // Arrange
        var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, MemoryCache, OrderValidator);
        var cartHeaderDto = new CartHeaderDto()
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
        };

        var cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeaderDto, [cartDetailsDto]);

        // Act
        var result = await handler.Handle(new CreateOrderRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(201);
        result.SuccessMessage.Should().Be("Заказ успешно создан");

        var getCreatedOrder = await Context.OrderHeaders.AsNoTracking()
            .FirstOrDefaultAsync(key => key.OrderHeaderId == result.Data!.OrderHeaderId);

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
    public async Task CreateOrderRequestHandler_FailOrWrong_Name()
    {
        // Arrange
        var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, MemoryCache, OrderValidator);
        var cartHeaderDto = new CartHeaderDto()
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
        };

        var cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeaderDto, [cartDetailsDto]);

        // Act
        var result = await handler.Handle(new CreateOrderRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Создать заказ невозможно");
        result.ValidationErrors.Should().Equal("Имя должно быть более 2 символов");
    }

    [Fact]
    public async Task CreateOrderRequestHandler_FailOrWrong_EmailAddress()
    {
        // Arrange
        var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, MemoryCache, OrderValidator);
        var cartHeaderDto = new CartHeaderDto()
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
        };

        var cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeaderDto, [cartDetailsDto]);

        // Act
        var result = await handler.Handle(new CreateOrderRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Создать заказ невозможно");
        result.ValidationErrors.Should().Equal("Введите действительный адрес электронной почты");
    }

    [Fact]
    public async Task CreateOrderRequestHandler_FailOrWrong_PhoneNumber()
    {
        // Arrange
        var handler = new CreateOrderRequestHandler(OrderHeader, Mapper, MemoryCache, OrderValidator);
        var cartHeaderDto = new CartHeaderDto()
        {
            CartHeaderId = 1,
            UserId = "TestuserId23",
            CouponCode = "5OFF",
            Discount = 10,
            CartTotal = 20,

            Name = "name",
            PhoneNumber = "37",
            Email = "email",
            Address = "test address",
        };

        var cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeaderDto, [cartDetailsDto]);

        // Act
        var result = await handler.Handle(new CreateOrderRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Создать заказ невозможно");
        result.SuccessMessage.Should().BeNullOrEmpty();
    }
}