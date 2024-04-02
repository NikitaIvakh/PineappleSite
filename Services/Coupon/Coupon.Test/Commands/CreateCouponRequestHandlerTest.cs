using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Domain.DTOs;
using Coupon.Test.Common;
using FluentAssertions;
using Shouldly;
using Stripe;
using Xunit;

namespace Coupon.Test.Commands;

public class CreateCouponRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task CreateCouponRequestHandler_Success()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test 1234",
            DiscountAmount: 40,
            MinAmount: 90
        );

        StripeConfiguration.ApiKey = "sk_test_51O90F4D1JYWWRL6F1K5vbfmQJeQuN8YRrNQYhq1I3l6OHyRqe6kzhS6wYYelu1YXtjftts7Ela0WDdmIafeGRS6n00AL3kb8tV";
        
        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.Data.Should().ShouldNotBeNull();
        result.StatusCode.Should().Be(201);
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_CouponCode_Max()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test 12341111111111111111111111111111111111111111111111111",
            DiscountAmount: 40,
            MinAmount: 90
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal("Строка не должна превышать 20 символов");
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_CouponCode_Min()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "T",
            DiscountAmount: 40,
            MinAmount: 90
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal("Строка должна превышать 3 символа");
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_DiscountAmount_Max()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test 1234",
            DiscountAmount: 401,
            MinAmount: 90
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal
        (
            "Сумма скидки не должна превышать 101 единицу", 
            "Скидка не должна превышать стоимость продукта"
        );
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_DiscountAmount_Min()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test coupon 12",
            DiscountAmount: 1,
            MinAmount: 90
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal("Сумма скидки должна превышать 2 единицы");
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_MinAmount_Max()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test 1234 2",
            DiscountAmount: 40,
            MinAmount: 1020
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal
        (
            "Цена товара должна быть ниже 101 единицы"
        );
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_MinAmount_Min()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "Test coupon 12",
            DiscountAmount: 19,
            MinAmount: 1
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal
        (
            "Скидка не должна превышать стоимость продукта", 
            "Сумма товара должна превышать 2 единицы"
        );
    }
    
    [Fact]
    public async Task CreateCouponRequestHandler_FailOrWrong_AllFields()
    {
        // Arrange
        var handler = new CreateCouponRequestHandler(Repository, CreateValidator, MemoryCache);
        var createCouponDto = new CreateCouponDto
        (
            CouponCode: "T",
            DiscountAmount: 190,
            MinAmount: 1009
        );

        // Act
        var result = await handler.Handle(new CreateCouponRequest(createCouponDto), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Купон не может быть создан");
        result.ValidationErrors.Should().Equal
        (
            "Строка должна превышать 3 символа", 
            "Сумма скидки не должна превышать 101 единицу", 
            "Цена товара должна быть ниже 101 единицы"
        );
    }
}