using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Coupon.Test.Commands
{
    public class CreateCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task CreareCouponRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, CreateLogger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test 123",
                DiscountAmount = 45,
                MinAmount = 67,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.Should().ShouldNotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.SuccessMessage.Should().Be("Купон успешно создан");
            result.ValidationErrors.Should().BeNull();


            var createCouponDto = await Context.Coupons.AsNoTracking().FirstOrDefaultAsync(key => key.CouponId == result.Data.CouponId);
            createCoupon.CouponCode.Should().Be("Test 123");
            createCoupon.DiscountAmount.Should().Be(45);
            createCoupon.MinAmount.Should().Be(67);
        }

        [Fact]
        public async Task CreateCouponRequestHandlerTest_MinAmount_Error()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, CreateLogger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 40,
                MinAmount = 1000,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Сумма для применения купона не валидна");
            result.SuccessMessage.Should().BeNull();
            result.ValidationErrors.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateCouponRequestHandlerTest_DiscountAmount_Error()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, CreateLogger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test",
                DiscountAmount = 56,
                MinAmount = 25,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Сумма скидки купона не может превышать стоимость продукта");
            result.SuccessMessage.Should().BeNull();
            result.ValidationErrors.Should().NotBeNull();
        }

        [Fact]
        public async Task CreateCouponRequestHandlerTest_CouponCode_Error()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, CreateLogger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "Test 12349 TestTestTestTestTestTestTestTestTestTestTest",
                DiscountAmount = 10,
                MinAmount = 25,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Код купона не валидный");
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().NotBeNull();
        }

        [Fact]
        public async Task CreareCouponRequestHandlerTest_CouponAlreadyExists()
        {
            // Arrange
            var handler = new CreateCouponRequestHandler(Repository, CreateLogger, Mapper, CreateValidator);
            var createCoupon = new CreateCouponDto
            {
                CouponCode = "10OFF",
                DiscountAmount = 10,
                MinAmount = 20,
            };

            // Act
            var result = await handler.Handle(new CreateCouponRequest
            {
                CreateCoupon = createCoupon,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Купон уже используется");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }
    }
}