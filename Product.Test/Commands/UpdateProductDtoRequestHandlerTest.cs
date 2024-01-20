using MediatR;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Core.Entities.Enum;
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
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper);
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
            result.ShouldBeOfType<Unit>();
        }

        [Fact]
        public async Task UpdateProductDtoRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new UpdateProductDtoRequestHandler(Context, Mapper);
            var updateProductDto = new UpdateProductDto
            {
                Id = 999,
                Name = "Name 1",
                Description = "Description 1",
                ProductCategory = ProductCategory.Snacks,
                Price = 20,
            };

            // Act && Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(new UpdateProductDtoRequest
            {
                UpdateProduct = updateProductDto,
            }, CancellationToken.None));
        }
    }
}