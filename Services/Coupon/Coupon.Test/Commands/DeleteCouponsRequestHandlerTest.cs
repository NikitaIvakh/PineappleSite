using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Coupon.Test.Commands;

public sealed class DeleteCouponsRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCouponsRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteCouponsRequestHandler(Repository, DeleteCouponsValidator, MemoryCache);
        var deleteCouponsDto = new DeleteCouponsDto
        (
            CouponIds: 
            [
                Guid.Parse("a70b2384-54bf-4c01-91be-689ba8dd1a31").ToString(), 
                Guid.Parse("284e4b19-fccf-4ac4-8b13-a26dcd9e2475").ToString()
            ]
        );

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteCouponsRequest(deleteCouponsDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(205);
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
            CouponIds: ["B4C4A067-1C55-42C8-993E-580DACC7A123", "AEDBA600-1D3D-4C77-BE31-186BC4E7A678"]
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