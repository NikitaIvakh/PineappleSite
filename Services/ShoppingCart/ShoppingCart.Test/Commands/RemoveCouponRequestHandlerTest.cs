using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class RemoveCouponRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task RemoveCouponRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new RemoveCouponRequestHandler(CartHeader, Mapper, MemoryCache);
        var cartHeader = new CartHeaderDto()
        {
            CartHeaderId = 1,
            UserId = "TestuserId23",
            CouponCode = "",
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
        var result = await handler.Handle(new RemoveCouponRequest(cartDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Купон успешно удален");
    }
}