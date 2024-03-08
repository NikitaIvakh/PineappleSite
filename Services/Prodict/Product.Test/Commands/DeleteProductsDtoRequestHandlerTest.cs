using FluentAssertions;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands
{
    public class DeleteProductsDtoRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task DeleteProductsDtoRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteProductsDtoRequestHandler(Repository, DeleteProductsValidator, DeleteListLogger, Mapper);
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
            result.IsSuccess.Should().BeTrue();
            result.SuccessMessage.Should().Be("Продукты успешно удалены");
            result.ValidationErrors.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task DeleteProductsDtoRequestHandlerTest_FailOrWrongIds()
        {
            // Arrange
            var handler = new DeleteProductsDtoRequestHandler(Repository, DeleteProductsValidator, DeleteListLogger, Mapper);
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
            result.ValidationErrors.Should().NotBeNullOrEmpty();
        }
    }
}