//using Coupon.Application.Features.Coupons.Handlers.Commands;
//using Coupon.Application.Features.Coupons.Requests.Commands;
//using Coupon.Domain.DTOs;
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
//            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, Logger, Mapper);
//            var updateCouponDto = new UpdateCouponDto
//            {
//                CouponId = 3,
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
//            result.ValidationErrors.ShouldBeNull();
//        }

//        [Fact]
//        public async Task UpdateCouponRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, Logger, Mapper);
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
//            result.ErrorMessage.ShouldBe("Внутренняя проблема сервера");
//        }

//        [Fact]
//        public async Task UpdateCouponRequestHandlerTest_ValidationError()
//        {
//            // Arrange
//            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, Logger, Mapper);
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
//            result.SuccessMessage.Should().BeNull();
//        }
//    }
//}