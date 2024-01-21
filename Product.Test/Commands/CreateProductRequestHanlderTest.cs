using Product.Application.DTOs.Products;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Response;
using Product.Core.Entities.Enum;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class CreateProductRequestHanlderTest : TestCommandHandler
    {
        [Fact]
        public async Task CreateProductRequestHanlderTest_Success()
        {
            // Arrange
            var handler = new CreateProductDtoRequestHandler(Context, Mapper, Validator);
            var createProductDto = new CreateProductDto
            {
                Name = "name",
                Description = "description",
                ProductCategory = ProductCategory.Drinks,
                Price = 24,
            };

            // Act
            var result = await handler.Handle(new CreateProductDtoRequest
            {
                CreateProduct = createProductDto,
            }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<ProductAPIResponse>();
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Продукт успешно добавлен");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task CreateProductRequestHanlderTest_FailOrWrongInputValidName()
        {
            // Arrange
            var handler = new CreateProductDtoRequestHandler(Context, Mapper, Validator);
            var createProductDto = new CreateProductDto
            {
                Name = "na",
                Description = "description description description",
                ProductCategory = ProductCategory.Drinks,
                Price = 24,
            };

            // Act
            var result = await handler.Handle(new CreateProductDtoRequest
            {
                CreateProduct = createProductDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Ошибка создания продукта");
            result.ValidationErrors.ShouldNotBeEmpty();
            result.ValidationErrors.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateProductRequestHanlderTest_FailOrWrongInputValidPrice()
        {
            // Arrange
            var handler = new CreateProductDtoRequestHandler(Context, Mapper, Validator);
            var createProductDto = new CreateProductDto
            {
                Name = "valid name",
                Description = "description description description exceeding the maximum length of 500 characters",
                ProductCategory = ProductCategory.Drinks,
                Price = 2000,
            };

            // Act
            var result = await handler.Handle(new CreateProductDtoRequest
            {
                CreateProduct = createProductDto,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe("Ошибка создания продукта");
            result.ValidationErrors.ShouldNotBeEmpty();
            result.ValidationErrors.ShouldNotBeNull();
        }
    }
}