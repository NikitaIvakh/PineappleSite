using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Test.Common;
using MediatR;
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
            var handler = new DeleteCouponRequestHandler(Context, Mapper);
            var deleteCouponId = 2;

            // Act
            var result = await handler.Handle(new DeleteCouponRequest
            {
                Id = deleteCouponId,
            }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteCouponRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new DeleteCouponRequestHandler(Context, Mapper);
            var deleteCouponId = 254657;

            // Assert && Act
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await handler.Handle(new DeleteCouponRequest
            {
                Id = deleteCouponId,
            }, CancellationToken.None));
        }
    }
}