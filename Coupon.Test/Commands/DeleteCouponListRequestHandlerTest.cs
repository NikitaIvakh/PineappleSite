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
//    public class DeleteCouponListRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task DeleteCouponListRequestHandlerTest_Success()
//        {
//            // Arrange
//            var validator = new DeleteListValidator();
//            var handler = new DeleteCouponListRequestHandler(Context, validator);
//            var deleteCouponList = new DeleteCouponListDto
//            {
//                CouponIds = [1, 2, 3],
//            };

//            // Act
//            var result = await handler.Handle(new DeleteCouponListRequest
//            {
//                DeleteCoupon = deleteCouponList,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeTrue();
//            result.Message.ShouldBe("Купоны успешно удалены");
//            result.ShouldBeOfType<BaseCommandResponse>();
//        }

//        [Fact]
//        public async Task DeleteCouponListRequestHandlerTest_EmptyList()
//        {
//            var validator = new DeleteListValidator();
//            var handler = new DeleteCouponListRequestHandler(Context, validator);
//            var deleteCouponList = new DeleteCouponListDto
//            {
//                CouponIds = [],
//            };

//            var result = await handler.Handle(new DeleteCouponListRequest
//            {
//                DeleteCoupon = deleteCouponList,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeFalse();
//        }
//    }
//}