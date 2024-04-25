using FluentAssertions;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Domain.Enum;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands;

public sealed class CreateProductRequestHanlderTest : TestCommandHandler
{
    [Fact]
    public async Task CreateProductRequestHanlderTest_Success()
    {
        // Arrange
        var handler = new CreateProductRequestHandler(Repository, CreateValidator, HttpContextAccessor, MemoryCache);
        var createProductDto = new CreateProductDto
        (
            Name: "name",
            Description: "description",
            ProductCategory: ProductCategory.Drinks,
            Price: 24,
            Avatar: null
        );

        // Act
        var result = await handler.Handle(new CreateProductRequest(createProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.SuccessMessage.ShouldBe("Продукт успешно добавлен");
        result.ValidationErrors.ShouldBeNull();
    }

    [Fact]
    public async Task CreateProductRequestHanlderTest_FailOrWrongInputValidName()
    {
        // Arrange
        var handler = new CreateProductRequestHandler(Repository, CreateValidator, HttpContextAccessor, MemoryCache);
        var createProductDto = new CreateProductDto
        (
            Name: "na",
            Description: "descriptiondescription",
            ProductCategory: ProductCategory.Drinks,
            Price: 24,
            Avatar: null
        );

        // Act
        var result = await handler.Handle(new CreateProductRequest(createProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.Should().Be("Продукт не может быть создан");
        result.ValidationErrors.ShouldBe(["Строка должна быть более 3 символов."]);
    }

    [Fact]
    public async Task CreateProductRequestHanlderTest_FailOrWrongInputValidDescription()
    {
        // Arrange
        var handler = new CreateProductRequestHandler(Repository, CreateValidator, HttpContextAccessor, MemoryCache);
        var createProductDto = new CreateProductDto
        (
            Name: "name345",
            Description:
            "This is not for an article, but for a short note. The optimal article size is 2000-3000 characters. " +
            "And 500 offhand, it will be about that much, you count the number of characters yourself, if you are not lazy.  " +
            "If you are reading these lines, surely you are interested in working with a specialist who is able to write sensible, vivid, unique and convincing texts? Agree that the success " +
            "of the entire project depends on the quality of writing articles. Therefore, it is so important to be able to interest the visitor, get into his position, draw a situation and show a " +
            "specific way to solve it. After all, what is missing from most articles on the Internet? Why do many of them look unfinished? It seems that something is missing. And what is it? " +
            "And this is the ability to turn ordinary words into advantages and benefits. This is the ability to fascinate from the first word and make you read the entire text to the last point. " +
            "And the most important thing is not just to read, but to completely soak up the idea and make a decision.",
            ProductCategory: ProductCategory.Drinks,
            Price: 24,
            Avatar: null
        );

        // Act
        var result = await handler.Handle(new CreateProductRequest(createProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.StatusCode.Should().Be(204);
        result.ErrorMessage.ShouldBe("Продукт не может быть создан");
        result.ValidationErrors.ShouldBe([
            "Строка не должна превышать 500 символов."
        ]);
    }

    [Fact]
    public async Task CreateProductRequestHanlderTest_FailOrWrongInputValidPrice()
    {
        // Arrange
        var handler = new CreateProductRequestHandler(Repository, CreateValidator, HttpContextAccessor, MemoryCache);
        var createProductDto = new CreateProductDto
        (
            Name: "na",
            Description: "descriptiondescription",
            ProductCategory: ProductCategory.Drinks,
            Price: 900000,
            Avatar: null
        );

        // Act
        var result = await handler.Handle(new CreateProductRequest(createProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ErrorMessage.ShouldBe("Продукт не может быть создан");
    }
}