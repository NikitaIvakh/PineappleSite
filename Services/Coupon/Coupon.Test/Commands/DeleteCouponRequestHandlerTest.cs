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
            var handler = new DeleteCouponRequestHandler(Repository, DeleteLogger, Mapper, DeleteValidator, MemoryCache);
            var deleteCoupon = new DeleteCouponDto
            {
                Id = 3,
            };

            // Act
            var result = await handler.Handle(new DeleteCouponRequest
            {
                DeleteCoupon = deleteCoupon,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().Be("Купон успешно удален");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteCouponRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new DeleteCouponRequestHandler(Repository, DeleteLogger, Mapper, DeleteValidator, MemoryCache);
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
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Купон не может быть удален");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }
    }
}