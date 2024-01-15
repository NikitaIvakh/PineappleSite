using Coupon.Application.DTOs;
using Coupon.Application.DTOs.Validator;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class CreareCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task CreareCouponRequestHandlerTest_Success()
        {
            // Arrange
            var validator = new CreateCouponDtoValidator(Context);
            var handler = new CreateCouponRequestHandler(Context, Mapper, validator);
            var createCouponDto = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 45,
                MinAmount = 67,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCouponDto = createCouponDto,
            }, CancellationToken.None);

            // Assert
            result.Should().ShouldNotBeNull();
            result.Message.ShouldBe("Купон успешно создан");
            result.IsSuccess.ShouldBeTrue();

            var createCoupon = await Context.Coupons.AsNoTracking().FirstOrDefaultAsync(key => key.CouponId == result.Id);
            createCoupon.Should().NotBeNull();
        }

        [Fact]
        public async Task CreareCouponRequestHandlerTest_ValidationError()
        {
            // Arrange
            var validator = new CreateCouponDtoValidator(Context);
            var handler = new CreateCouponRequestHandler(Context, Mapper, validator);
            var createCouponDto = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 4567876,
                MinAmount = 676578768,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCouponDto = createCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
        }

        [Fact]
        public async Task CreareCouponRequestHandlerTest_DiscountAmount_Fail()
        {
            // Arrange
            var validator = new CreateCouponDtoValidator(Context);
            var handler = new CreateCouponRequestHandler(Context, Mapper, validator);
            var createCouponDto = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 56,
                MinAmount = 25,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCouponDto = createCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Ошибка при создании купона");
        }
    }
}