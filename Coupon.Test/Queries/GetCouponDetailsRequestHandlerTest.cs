using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Coupon.Test.Queries
{
    public class GetCouponDetailsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetCouponDetailsRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetCouponDetailsRequestHandler(Repository, GetLogger);
            int couponId = 5;

            // Act
            var result = await handler.Handle(new GetCouponDetailsRequest
            {
                Id = couponId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.ShouldBeNull();

            var getCoupon = await Context.Coupons.AsNoTracking().FirstOrDefaultAsync(key => key.CouponId == result.Data.CouponId);
            getCoupon.CouponId.Should().Be(5);
            getCoupon.CouponCode.Should().Be("30OFF");
            getCoupon.DiscountAmount.Should().Be(30);
            getCoupon.MinAmount.Should().Be(40);
        }

        [Fact]
        public async Task GetCouponDetailsRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new GetCouponDetailsRequestHandler(Repository, GetLogger);
            var couponId = 999;

            // Act
            var result = await handler.Handle(new GetCouponDetailsRequest
            {
                Id = couponId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.ShouldBe("Купон не найден");
            result.SuccessMessage.Should().BeNullOrEmpty();

        }
    }
}