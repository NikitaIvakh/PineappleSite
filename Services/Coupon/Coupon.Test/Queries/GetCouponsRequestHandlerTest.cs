using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
using Xunit;

namespace Coupon.Test.Queries;

public sealed class GetCouponsRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetCouponListRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetCouponsRequestHandler(Repository, MemoryCache, Mapper);

        // Act
        var result = await handler.Handle(new GetCouponsRequest(), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(200);
        result.SuccessMessage.Should().Be("Купоны успешно получены");
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }
}