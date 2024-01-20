using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
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
            var handler = new CreateProductDtoRequestHandler(Context, Mapper);
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
            result.ShouldBeOfType<int>();
        }
    }
}