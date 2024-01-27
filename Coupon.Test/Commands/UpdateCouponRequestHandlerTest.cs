//using Coupon.Application.DTOs;
//using Coupon.Application.DTOs.Validator;
//using Coupon.Application.Exceptions;
//using Coupon.Application.Features.Coupons.Handlers.Commands;
//using Coupon.Application.Features.Coupons.Requests.Commands;
//using Coupon.Test.Common;
//using FluentAssertions;
//using Shouldly;
//using Xunit;

//namespace Coupon.Test.Commands
//{
//    public class UpdateCouponRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task UpdateCouponRequestHandlerTest_Success()
//        {
//            // Arrange
//            var validator = new UpdateValidator(Context);
//            var handler = new UpdateCouponRequestHandler(Context, Mapper, validator);
//            var updateCouponDto = new UpdateCouponDto
//            {
//                CouponId = 2,
//                CouponCode = "Test",
//                DiscountAmount = 25,
//                MinAmount = 45,
//            };

//            // Act
//            var result = await handler.Handle(new UpdateCouponRequest
//            {
//                UpdateCoupon = updateCouponDto,
//            }, CancellationToken.None);

//            // Assert
//            result.Should().NotBeNull();
//            result.Message.ShouldBe("Купон успешно обновлен");
//            result.ValidationErrors.ShouldBeNull();
//        }

//        [Fact]
//        public async Task UpdateCouponRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var validator = new UpdateValidator(Context);
//            var handler = new UpdateCouponRequestHandler(Context, Mapper, validator);
//            var updateCouponDto = new UpdateCouponDto
//            {
//                CouponId = 999,
//                CouponCode = "Test",
//                DiscountAmount = 24,
//                MinAmount = 45,
//            };

//            // Act
//            var result = await handler.Handle(new UpdateCouponRequest
//            {
//                UpdateCoupon = updateCouponDto,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeFalse();
//            result.Message.ShouldBe("Sequence contains no elements");
//        }

//        [Fact]
//        public async Task UpdateCouponRequestHandlerTest_ValidationError()
//        {
//            // Arrange
//            var validator = new UpdateValidator(Context);
//            var handler = new UpdateCouponRequestHandler(Context, Mapper, validator);
//            var updateCouponDto = new UpdateCouponDto
//            {
//                CouponId = 1,
//                CouponCode = "Test",
//                DiscountAmount = 23456,
//                MinAmount = 56745,
//            };

//            // Act
//            var result = await handler.Handle(new UpdateCouponRequest
//            {
//                UpdateCoupon = updateCouponDto,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeFalse();
//            result.Message.ShouldBe("Ошибка при обновлении купона");
//        }
//    }
//}