//using Coupon.Application.Exceptions;
//using Coupon.Application.Features.Coupons.Handlers.Queries;
//using Coupon.Application.Features.Coupons.Requests.Queries;
//using Coupon.Test.Common;
//using Shouldly;
//using Xunit;

//namespace Coupon.Test.Queries
//{
//    public class GetCouponDetailsRequestHandlerTest : TestQueryHandler
//    {
//        [Fact]
//        public async Task GetCouponDetailsRequestHandlerTest_Success()
//        {
//            // Arrange
//            var handler = new GetCouponDetailsRequestHandler(Context, Mapper);
//            int couponId = 3;

//            // Act
//            var result = await handler.Handle(new GetCouponDetailsRequest
//            {
//                Id = couponId,
//            }, CancellationToken.None);

//            // Assert
//            result.CouponId = couponId;
//            result.CouponCode.ShouldBe("10OFF");
//            result.DiscountAmount.ShouldBe(10);
//            result.MinAmount.ShouldBe(20);
//        }

//        [Fact]
//        public async Task GetCouponDetailsRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var handler = new GetCouponDetailsRequestHandler(Context, Mapper);
//            var couponId = 999;

//            // Act
//            var exception = await Record.ExceptionAsync(async () => await handler.Handle(new GetCouponDetailsRequest
//            {
//                Id = couponId,
//            }, CancellationToken.None));

//            // Assert
//            exception.ShouldBeOfType<NotFoundException>();
//            exception.Message.ShouldBe($"CouponEntity ({couponId}) не найдено!");
//        }
//    }
//}