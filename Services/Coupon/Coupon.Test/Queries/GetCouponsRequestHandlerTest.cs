using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
using Xunit;

namespace Coupon.Test.Queries
{
    public class GetCouponsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetCouponListRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetCouponsRequestHandler(Repository, MemoryCache);

            // Act
            var result = await handler.Handle(new GetCouponsRequest(), CancellationToken.None);

            // Assert
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
        }
    }
}