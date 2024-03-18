using FluentAssertions;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands
{
    public class RemoveShoppingCartDetailsRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task RemoveShoppingCartDetailsRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new RemoveShoppingCartDetailsRequestHandler(CartHeader, CartDetails, Mapper, MemoryCache);
            var productId = 2;

            // Act
            var result = await handler.Handle(new RemoveShoppingCartDetailsRequest
            {
                ProductId = productId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Внутренняя проблемы сервера");
            result.ErrorCode.Should().Be(500);
        }
    }
}