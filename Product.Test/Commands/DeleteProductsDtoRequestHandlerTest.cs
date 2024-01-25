using Microsoft.AspNetCore.Http;
using Product.Application.DTOs.Products;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class DeleteProductsDtoRequestHandlerTest : TestCommandHandler
    {
        public DeleteProductsDtoRequestHandlerTest(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        [Fact]
        public async Task DeleteProductsDtoRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteProductsDtoRequestHandler(Context, DeleteProductsValidator);
            var deleteProducts = new DeleteProductsDto
            {
                ProductIds = new List<int> { 1, 2 },
            };

            // Act
            var result = await handler.Handle(new DeleteProductsDtoRequest
            {
                DeleteProducts = deleteProducts,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Message.ShouldBe("Продукты успешно удалены");
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task DeleteProductsDtoRequestHandlerTest_FailOrWrongIds()
        {
            // Arrange
            var handler = new DeleteProductsDtoRequestHandler(Context, DeleteProductsValidator);
            var deleteProducts = new DeleteProductsDto
            {
                ProductIds = new List<int>(),
            };


            // Act
            var result = await handler.Handle(new DeleteProductsDtoRequest
            {
                DeleteProducts = deleteProducts,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeFalse();
            result.ValidationErrors.ShouldNotBeEmpty();
            result.ValidationErrors.ShouldNotBeNull();
        }
    }
}