using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
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
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Context, Mapper);
            var couponCode = "10OFF";

            // Act
            var result = await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None);

            // Assert
            result.CouponId.ShouldBe(3);
            result.CouponCode.ShouldBe("10OFF");
            result.DiscountAmount.ShouldBe(10);
            result.MinAmount.ShouldBe(20);
        }

        [Fact]
        public async Task GetCouponDetailsByCouponNameRequestHandlerTest_FailOrWrongCouponCode()
        {
            // Arrange
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Context, Mapper);
            var couponCode = "101OFF";

            // Act
            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None));

            // Assert
            exception.ShouldBeOfType<NotFoundException>();
            exception.Message.ShouldBe($"CouponEntity ({couponCode}) не найдено!");
        }
    }
}