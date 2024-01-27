//using Coupon.Application.DTOs;
//using Coupon.Application.DTOs.Validator;
//using Coupon.Application.Features.Coupons.Handlers.Commands;
//using Coupon.Application.Features.Coupons.Requests.Commands;
//using Coupon.Application.Response;
//using Coupon.Test.Common;
//using Shouldly;
//using Xunit;

//namespace Coupon.Test.Commands
//{
//    public class DeleteCouponRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task DeleteCouponRequestHandlerTest_Success()
//        {
//            // Arrange
//            var validator = new DeleteValidator();
//            var handler = new DeleteCouponRequestHandler(Context, validator);
//            var deleteCoupon = new DeleteCouponDto
//            {
//                Id = 2,
//            };

//            // Act
//            var result = await handler.Handle(new DeleteCouponRequest
//            {
//                DeleteCoupon = deleteCoupon,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeTrue();
//            result.ShouldBeOfType<BaseCommandResponse>();
//        }

//        [Fact]
//        public async Task DeleteCouponRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var validator = new DeleteValidator();
//            var handler = new DeleteCouponRequestHandler(Context, validator);
//            var deleteCoupon = new DeleteCouponDto
//            {
//                Id = 324542,
//            };

//            // Act
//            var result = await handler.Handle(new DeleteCouponRequest
//            {
//                DeleteCoupon = deleteCoupon,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeFalse();
//        }
//    }
//}