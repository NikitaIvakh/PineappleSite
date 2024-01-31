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
//            var handler = new GetCouponDetailsRequestHandler(Repository, Logger);
//            int couponId = 1;

//            // Act
//            var result = await handler.Handle(new GetCouponDetailsRequest
//            {
//                Id = couponId,
//            }, CancellationToken.None);

//            // Assert
//            result.ValidationErrors.ShouldBeNull();
//        }

//        [Fact]
//        public async Task GetCouponDetailsRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var handler = new GetCouponDetailsRequestHandler(Repository, Logger);
//            var couponId = 999;

//            // Act
//            var result = await handler.Handle(new GetCouponDetailsRequest
//            {
//                Id = couponId,
//            }, CancellationToken.None);

//            // Assert
//            result.ErrorMessage.ShouldBe("Внутренняя проблема сервера");
            
//        }
//    }
//}