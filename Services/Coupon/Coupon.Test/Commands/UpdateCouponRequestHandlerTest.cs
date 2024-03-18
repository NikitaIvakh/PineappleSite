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
    public class UpdateCouponRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task UpdateCouponRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "Test",
                DiscountAmount = 25,
                MinAmount = 45,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().Be("Купон успешно обновлен");
            result.ValidationErrors.ShouldBeNull();

            var updatedCoupon = await Context.Coupons.AsNoTracking().FirstOrDefaultAsync(key => key.CouponId == result.Data.CouponId);
            updatedCoupon.CouponId.Should().Be(3);
            updatedCoupon.CouponCode.Should().Be("Test");
            updatedCoupon.DiscountAmount.Should().Be(25);
            updatedCoupon.MinAmount.Should().Be(45);
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 999,
                CouponCode = "Test",
                DiscountAmount = 24,
                MinAmount = 45,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.ErrorMessage.ShouldBe("Такого купона не существует");
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_CouponCodeIsNotValid()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
                DiscountAmount = 10,
                MinAmount = 30,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ErrorMessage.Should().Be("Купон не может быть обновлен");
            result.ValidationErrors.ShouldBe(["Строка не должна превышать 20 символов"]);
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_DiscountAmountIsNotValid()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "Test",
                DiscountAmount = 90,
                MinAmount = 30,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ErrorMessage.Should().Be("Купон не может быть обновлен");
            result.ValidationErrors.ShouldBe(["Скидка не должна превышать стоимость продукта"]);
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_MinAmountIsNotValid()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "Test",
                DiscountAmount = 90,
                MinAmount = 400,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ErrorMessage.Should().Be("Купон не может быть обновлен");
            result.ValidationErrors.ShouldBe(["Цена товара должна быть ниже 101 единицы"]);
        }

        [Fact]
        public async Task UpdateCouponRequestHandlerTest_AllPropertiesIsNotValid_Without_CouponId()
        {
            // Arrange
            var handler = new UpdateCouponRequestHandler(Repository, UpdateValidator, UpdateLogger, Mapper, MemoryCache);
            var updateCouponDto = new UpdateCouponDto
            {
                CouponId = 3,
                CouponCode = "TestTestTestTestTestTestTestTestTestTestTestTestTestTestTestTest",
                DiscountAmount = 200,
                MinAmount = 100,
            };

            // Act
            var result = await handler.Handle(new UpdateCouponRequest
            {
                UpdateCoupon = updateCouponDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.SuccessMessage.Should().BeNullOrEmpty();
            result.ErrorMessage.Should().Be("Купон не может быть обновлен");
            result.ValidationErrors.ShouldBe([
                "Строка не должна превышать 20 символов",
                "Сумма скидки не должна превышать 101 единицу",
                "Скидка не должна превышать стоимость продукта"]);
        }
    }
}