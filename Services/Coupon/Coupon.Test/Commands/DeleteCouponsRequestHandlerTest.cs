using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coupon.Test.Commands;

public class DeleteCouponsRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCouponsRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteCouponsRequestHandler(Repository, DeleteCouponsValidator, MemoryCache);
        var deleteCouponsDto = new DeleteCouponsDto
        (
            CouponIds: [3, 4, 5]
        );

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
        
        // Act
        var result = await handler.Handle(new DeleteCouponsRequest(deleteCouponsDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(200);
        result.SuccessMessage.Should().Be("Купоны успешно удалены");
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public async Task DeleteCouponsRequestHandlerTest_FailOrWrong_CouponIds()
    {
        // Arrange
        var handler = new DeleteCouponsRequestHandler(Repository, DeleteCouponsValidator, MemoryCache);
        var deleteCouponsDto = new DeleteCouponsDto
        (
            CouponIds: [7, 8, 7]
        );

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }
        
        // Act
        var result = await handler.Handle(new DeleteCouponsRequest(deleteCouponsDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Купоны не найдены");
        result.ValidationErrors.Should().Equal("Купоны не найдены");
    }
}