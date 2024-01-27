using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using Shouldly;
using Xunit;

namespace Coupon.Test.Queries
{
    public class GetCouponDetailsByCouponNameRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetCouponDetailsByCouponNameRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Repository, Logger);
            var couponCode = "10OFF";

            // Act
            var result = await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.ShouldBeNull();

        }

        [Fact]
        public async Task GetCouponDetailsByCouponNameRequestHandlerTest_FailOrWrongCouponCode()
        {
            // Arrange
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Repository, Logger);
            var couponCode = "101OFF";

            // Act
            var result = await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.ShouldBeNull();
        }
    }
}