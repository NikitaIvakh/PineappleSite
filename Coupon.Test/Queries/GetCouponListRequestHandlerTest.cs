using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
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
            var handler = new GetCouponListRequestHandler(Repository, GetListLogger);

            // Act
            var result = await handler.Handle(new GetCouponListRequest(), CancellationToken.None);

            // Assert
            result.Count.Should().Be(5);
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
        }
    }
}