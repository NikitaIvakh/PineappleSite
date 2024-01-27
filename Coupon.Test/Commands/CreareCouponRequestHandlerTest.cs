using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class CreareCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task CreareCouponRequestHandlerTest_Success_()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, Logger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test 123",
                DiscountAmount = 45,
                MinAmount = 67,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }

        [Fact]
        public async Task CreareCouponRequestHandlerTest_MinAmount_Error()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, Logger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 4567876,
                MinAmount = 676578768,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }

        [Fact]
        public async Task CreareCouponRequestHandlerTest_DiscountAmount_Error()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, Logger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 56,
                MinAmount = 25,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }
    }
}