using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using Coupon.Test.Common;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class DeleteCouponListRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task DeleteCouponListRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteCouponListRequestHandler(Repository, Logger, Mapper, DeleteListValidator);
            var deleteCouponList = new DeleteCouponListDto
            {
                CouponIds = [1, 2, 3],
            };

            // Act
            var result = await handler.Handle(new DeleteCouponListRequest
            {
                DeleteCoupon = deleteCouponList,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }

        [Fact]
        public async Task DeleteCouponListRequestHandlerTest_EmptyList()
        {
            var handler = new DeleteCouponListRequestHandler(Repository, Logger, Mapper, DeleteListValidator);
            var deleteCouponList = new DeleteCouponListDto
            {
                CouponIds = [],
            };

            var result = await handler.Handle(new DeleteCouponListRequest
            {
                DeleteCoupon = deleteCouponList,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
        }
    }
}