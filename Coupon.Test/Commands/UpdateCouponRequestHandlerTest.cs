using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Test.Common;
using FluentAssertions;
using Xunit;

namespace Coupon.Test.Commands
{
    public class UpdateCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task UpdateCouponRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Context, Mapper);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "Test",
                DiscountAmount = 56,
                MinAmount = 45,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Context, Mapper);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 999,
                CouponCode = "Test",
                DiscountAmount = 56,
                MinAmount = 45,
            };

            // Assert && Act
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None));
        }
    }
}