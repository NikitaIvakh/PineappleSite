using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
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
            var handler = new DeleteProductDtoRequestHandler(Context, Mapper);
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
            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task DeleteProductDtoRequestHandlerTest_FailOrWrong()
        {
            // Arrange
            var handler = new DeleteProductDtoRequestHandler(Context, Mapper);
            var deleteProductDto = new DeleteProductDto
            {
                Id = 999,
            };

            // Act &7 Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(new DeleteProductDtoRequest
            {
                DeleteProduct = deleteProductDto
            }, CancellationToken.None));
        }
    }
}