using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class DeleteCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task DeleteCouponRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteCouponRequestHandler(Repository, Logger, Mapper, DeleteValidator);
            var deleteCoupon = new DeleteCouponDto
            {
                Id = 1,
            };

            // Act
            var result = await handler.Handle(new DeleteCouponRequest
            {
                DeleteCoupon = deleteCoupon,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCouponRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new DeleteCouponRequestHandler(Repository, Logger, Mapper, DeleteValidator);
            var deleteCoupon = new DeleteCouponDto
            {
                Id = 88,
            };

            // Act
            var result = await handler.Handle(new DeleteCouponRequest
            {
                DeleteCoupon = deleteCoupon,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }
    }
}