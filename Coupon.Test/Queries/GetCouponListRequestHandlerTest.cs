using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using Xunit;

namespace Coupon.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetCouponListRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetCouponListRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetCouponListRequestHandler(Context, Mapper);

            // Act
            var result = await handler.Handle(new GetCouponListRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CouponDto>>(result);
            Assert.Equal(5, result.Count());
        }
    }
}