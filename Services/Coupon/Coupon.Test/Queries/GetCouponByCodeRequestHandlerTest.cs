using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Test.Common;
using FluentAssertions;
using Xunit;

namespace Coupon.Test.Queries;

public class GetCouponByCodeRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GtCouponByCodeRequestHandler_Success()
    {
        // Arrange
        var handler = new GetCouponByCodeRequestHandler(Repository, MemoryCache);
        const string couponCode = "10OFF";

        // Act
        var result = await handler.Handle(new GetCouponByCodeRequest(couponCode), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty(); 
        result.ValidationErrors.Should().BeNullOrEmpty(); 
    }
    
    [Fact]
    public async Task GtCouponByCodeRequestHandler_FailOrWrong_CouponCode()
    {
        // Arrange
        var handler = new GetCouponByCodeRequestHandler(Repository, MemoryCache);
        const string couponCode = "10O1FF";

        // Act
        var result = await handler.Handle(new GetCouponByCodeRequest(couponCode), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Купон не найден");
        result.SuccessMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().Equal("Купон не найден");
    }
}