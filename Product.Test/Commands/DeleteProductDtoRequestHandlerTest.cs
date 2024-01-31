using FluentAssertions;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Domain.ResultProduct;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class DeleteProductDtoRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task DeleteProductDtoRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteProductDtoRequestHandler(Repository, DeleteValidator, DeleteLogger, Mapper);
            var deleteProductDto = new DeleteProductDto
            {
                Id = 3,
            };

            // Act
            var result = await handler.Handle(new DeleteProductDtoRequest
            {
                DeleteProduct = deleteProductDto
            }, CancellationToken.None);

            // Assert
            result.ShouldBeOfType<Result<ProductDto>>();
            result.IsSuccess.ShouldBeTrue();
            result.SuccessMessage.Should().Be("Продукт успешно удален");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteProductDtoRequestHandlerTest_FailOrWrong()
        {
            // Arrange
            var handler = new DeleteProductDtoRequestHandler(Repository, DeleteValidator, DeleteLogger, Mapper);
            var deleteProductDto = new DeleteProductDto
            {
                Id = 999,
            };

            // Act
            var result = await handler.Handle(new DeleteProductDtoRequest
            {
                DeleteProduct = deleteProductDto
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Продукт не найден");
            result.ValidationErrors.ShouldBeNull();
        }
    }
}