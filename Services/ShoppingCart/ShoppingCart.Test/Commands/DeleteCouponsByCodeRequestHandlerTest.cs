using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public class DeleteCouponsByCodeRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCouponsByCodeRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteCouponsByCodeRequestHandler(CartHeader);
        var deleteCouponsDto = new DeleteCouponsByCodeDto(["5OFF"]);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteCouponsByCodeRequest(deleteCouponsDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(203);
        result.SuccessMessage.Should().Be("Купоны успешно обновлены");
    }
}