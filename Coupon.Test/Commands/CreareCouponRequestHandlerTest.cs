using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class CreareCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task CreareCouponRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Context, Mapper);
            var createCouponDto = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 45,
                MinAmount = 67,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCouponDto = createCouponDto,
            }, CancellationToken.None);

            // Assert
            result.Should().ShouldNotBeNull();

            var createCoupon = await Context.Coupons.AsNoTracking().FirstOrDefaultAsync(key => key.CouponId == result);
            createCoupon.Should().NotBeNull();
        }
    }
}