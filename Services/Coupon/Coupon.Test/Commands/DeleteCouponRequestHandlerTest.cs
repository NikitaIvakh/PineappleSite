using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coupon.Test.Commands;

public sealed class DeleteCouponRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCouponRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteCouponRequestHandler(Repository, DeleteValidator, MemoryCache);
        const int couponId = 3;
        var deleteCouponDto = new DeleteCouponDto(CouponId: couponId);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteCouponRequest(deleteCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Купон успешно удален");
    }
    
    [Fact]
    public async Task DeleteCouponRequestHandlerTest_FailOrWrong_CouponId()
    {
        // Arrange
        var handler = new DeleteCouponRequestHandler(Repository, DeleteValidator, MemoryCache);
        const int couponId = 34;
        var deleteCouponDto = new DeleteCouponDto(CouponId: couponId);

        // Act
        var result = await handler.Handle(new DeleteCouponRequest(deleteCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Купон не найден");
        result.ValidationErrors.Should().Equal("Купон не найден");
    }
}