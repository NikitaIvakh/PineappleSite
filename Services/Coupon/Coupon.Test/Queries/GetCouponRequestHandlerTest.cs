using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Coupon.Test.Queries;

public sealed class GetCouponRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetCouponRequestHandlerTest_Success()
    {
        var handler = new GetCouponRequestHandler(Repository, MemoryCache);
        const int couponId = 5;

        // Act
        var result = await handler.Handle(new GetCouponRequest(couponId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.ShouldBeNull();

        var getCoupon = await Repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponId == result.Data!.CouponId);
        getCoupon!.CouponId.Should().Be(5);
        getCoupon.CouponCode.Should().Be("30OFF");
        getCoupon.DiscountAmount.Should().Be(30);
        getCoupon.MinAmount.Should().Be(40);
    }
    
    [Fact]
    public async Task GetCouponRequestHandlerTest_FailOrWrong_CouponId()
    {
        var handler = new GetCouponRequestHandler(Repository, MemoryCache);
        const int couponId = 999;

        // Act
        var result = await handler.Handle(new GetCouponRequest(couponId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.ShouldBe("Купон не найден");
        result.ValidationErrors.Should().Equal("Купон не найден");
        result.SuccessMessage.Should().BeNullOrEmpty();
    }
}