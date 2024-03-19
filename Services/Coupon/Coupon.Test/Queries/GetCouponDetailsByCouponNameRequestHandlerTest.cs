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
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Repository, GetByCodeLogger, MemoryCache);
            var couponCode = "10OFF";

            // Act
            var result = await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty(); 
            result.ValidationErrors.Should().BeNullOrEmpty(); 
        }

        [Fact]
        public async Task GetCouponDetailsByCouponNameRequestHandlerTest_FailOrWrongCouponCode()
        {
            // Arrange
            var handler = new GetCouponDetailsByCouponNameRequestHandler(Repository, GetByCodeLogger, MemoryCache);
            var couponCode = "101OFF";

            // Act
            var result = await handler.Handle(new GetCouponDetailsByCouponNameRequest
            {
                CouponCode = couponCode,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Купон не найден");
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().Equal("Купон не найден");
        }
    }
}