using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Response;
using Product.Core.Entities.Enum;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class UpdateProductDtoRequestHandlerTest : TestCommandHandler
    {
        public UpdateProductDtoRequestHandlerTest(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper, UpdateValidator, HttpContextAccessor);
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
            result.ShouldBeOfType<ProductAPIResponse>();
            result.Id.ShouldBe(4);
            result.IsSuccess.ShouldBeTrue();
            result.Message = "Продукт успешно обновлен";
            result.ValidationErrors.ShouldBeNull();

            var updateProduct = await Context.Products.AsNoTracking().FirstOrDefaultAsync(key => key.Id == result.Id);
            updateProduct.Should().NotBeNull();
            updateProduct?.Name.ShouldBe("Name 1");
            updateProduct?.Description.ShouldBe("Description 1");
            updateProduct?.ProductCategory.ShouldBe(ProductCategory.Snacks);
            updateProduct?.Price.ShouldBe(20);
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper, UpdateValidator, HttpContextAccessor);
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
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe($"У продукта ({updateProductDto.Name}) не существует идкетификатора:  ({updateProductDto.Id}) не найдено");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongInputValidName()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper, UpdateValidator, HttpContextAccessor);
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
            result.ShouldBeOfType<ProductAPIResponse>();
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Ошибка обновления продукта");
            result.ValidationErrors.ShouldNotBeEmpty();
            result.ValidationErrors.ShouldNotBeNull();
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongInputValidDescription()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper, UpdateValidator, HttpContextAccessor);
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
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Ошибка обновления продукта");
            result.ValidationErrors.ShouldNotBeEmpty();
            result.ValidationErrors.ShouldNotBeNull();
        }
    }
}