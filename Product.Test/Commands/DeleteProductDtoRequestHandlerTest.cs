using MediatR;
using Microsoft.AspNetCore.Http;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Response;
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
            var handler = new DeleteProductDtoRequestHandler(Context, DeleteValidator);
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
            result.ShouldBeOfType<ProductAPIResponse>();
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Продукт успешно удален");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteProductDtoRequestHandlerTest_FailOrWrong()
        {
            // Arrange
            var handler = new DeleteProductDtoRequestHandler(Context, DeleteValidator);
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
            result.IsSuccess.ShouldBeFalse();
            result.Message.ShouldBe($"Продукта c идентификатором: ({deleteProductDto.Id}) не найдено");
            result.ValidationErrors.ShouldBeNull();
        }
    }
}