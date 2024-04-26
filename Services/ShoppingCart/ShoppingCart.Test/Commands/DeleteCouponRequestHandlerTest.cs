using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class DeleteCouponRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCouponRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteCouponRequestHandler(CartHeader, Mapper);
        const string couponCode = "5OFF";
        var deleteCouponDto = new DeleteCouponDto(couponCode);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var resulty = await handler.Handle(new DeleteCouponRequest(deleteCouponDto), CancellationToken.None);

        // Assert
        resulty.IsSuccess.Should().BeTrue();
        resulty.StatusCode.Should().Be(203);
        resulty.SuccessMessage.Should().Be("Купоны успешно удалены");
    }
}