using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Domain.Enum;
using Product.Domain.ResultProduct;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class UpdateProductDtoRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Repository, UpdateLogger, Mapper, UpdateValidator, HttpContextAccessor, MemoryCache);
            var updateProductDto = new UpdateProductDto
            {
                Id = 4,
                Name = "Name 1",
                Description = "Description 1",
                ProductCategory = ProductCategory.Snacks,
                Price = 20,
            };

            // Act
            var result = await handler.Handle(new UpdateProductDtoRequest
            {
                UpdateProduct = updateProductDto
            }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<ProductDto>>();
            result.Data.Id.ShouldBe(4);
            result.IsSuccess.ShouldBeTrue();
            result.SuccessMessage = "Продукт успешно обновлен";
            result.ValidationErrors.Should().BeNullOrEmpty();

            var updateProduct = await Context.Products.AsNoTracking().FirstOrDefaultAsync(key => key.Id == result.Data.Id);
            updateProduct.Should().NotBeNull();
            updateProduct?.Name.Should().Be("Name 1");
            updateProduct?.Description.Should().Be("Description 1");
            updateProduct?.ProductCategory.Should().Be(ProductCategory.Snacks);
            updateProduct?.Price.Should().Be(20);
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Repository, UpdateLogger, Mapper, UpdateValidator, HttpContextAccessor, MemoryCache);
            var updateProductDto = new UpdateProductDto
            {
                Id = 999,
                Name = "Name 1",
                Description = "Description 1",
                ProductCategory = ProductCategory.Snacks,
                Price = 20,
            };

            // Act 
            var result = await handler.Handle(new UpdateProductDtoRequest
            {
                UpdateProduct = updateProductDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Такого продукта не существует");
            result.ValidationErrors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongInputValidName()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Repository, UpdateLogger, Mapper, UpdateValidator, HttpContextAccessor, MemoryCache);
            var updateProductDto = new UpdateProductDto
            {
                Id = 4,
                Name = "te",
                Description = "description",
                ProductCategory = ProductCategory.Snacks,
                Price = 20,
            };

            // Act
            var result = await handler.Handle(new UpdateProductDtoRequest
            {
                UpdateProduct = updateProductDto,
            }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<ProductDto>>();
            result.IsSuccess.ShouldBeFalse();
            result.ErrorMessage.ShouldBe("Продукт не может быть обновлен");
            result.ValidationErrors.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongInputValidDescription()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Repository, UpdateLogger, Mapper, UpdateValidator, HttpContextAccessor, MemoryCache);
            var updateProductDto = new UpdateProductDto
            {
                Id = 4,
                Name = "Update name",
                Description = "This is not for an article, but for a short note. The optimal article size is 2000-3000 characters. " +
                "And 500 offhand, it will be about that much, you count the number of characters yourself, if you are not lazy.  " +
                "If you are reading these lines, surely you are interested in working with a specialist who is able to write sensible, vivid, unique and convincing texts? Agree that the success " +
                "of the entire project depends on the quality of writing articles. Therefore, it is so important to be able to interest the visitor, get into his position, draw a situation and show a " +
                "specific way to solve it. After all, what is missing from most articles on the Internet? Why do many of them look unfinished? It seems that something is missing. And what is it? " +
                "And this is the ability to turn ordinary words into advantages and benefits. This is the ability to fascinate from the first word and make you read the entire text to the last point. " +
                "And the most important thing is not just to read, but to completely soak up the idea and make a decision.",
                ProductCategory = ProductCategory.Drinks,
                Price = 20,
            };

            // Act
            var result = await handler.Handle(new UpdateProductDtoRequest
            {
                UpdateProduct = updateProductDto,
            }, CancellationToken.None);

            // Asert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.ShouldBe("Продукт не может быть обновлен");
            result.ValidationErrors.Should().NotBeNullOrEmpty();
        }
    }
}