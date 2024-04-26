using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class ApplyCouponRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task ApplyCouponRequestHandlerTestTest_Success()
    {
        // Arrange
        var handler = new ApplyCouponRequestHandler(CartHeader, Mapper, CouponService, MemoryCache);
        var cartHeader = new CartHeaderDto()
        {
            CartHeaderId = 1,
            UserId = "TestuserId23",
            CouponCode = "5OFF",
            Discount = 10,
            CartTotal = 20
        };

        var cartDetails = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeader, [cartDetails]);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new ApplyCouponRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(203);
        result.SuccessMessage.Should().Be("Купон успешно применен");
    }

    [Fact]
    public async Task ApplyCouponRequestHandlerTestTest_FailOrWrong_CouponCode()
    {
        // Arrange
        var handler = new ApplyCouponRequestHandler(CartHeader, Mapper, CouponService, MemoryCache);
        var cartHeader = new CartHeaderDto()
        {
            CartHeaderId = 1,
            UserId = "TestuserId23",
            CouponCode = "1235OFF",
            Discount = 10,
            CartTotal = 20
        };

        var cartDetails = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 1,
            ProductId = 3,
            Count = 1,
        };

        var cartDto = new CartDto(cartHeader, [cartDetails]);

        // Act
        var result = await handler.Handle(new ApplyCouponRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Купон не найден");
        result.ValidationErrors.Should().Equal("Купон не найден");
    }
}