using ShoppingCart.Application.Features.Commands.Handlers;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Application.Response;
using ShoppingCart.Test.Common;
using Shouldly;
using Xunit;

namespace ShoppingCart.Test.Commands
{
    public class RemoveCartRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task RemoveCartRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new RemoveCartRequestHandlerc(Context, Context, Mapper);
            var cartDetailsId = 1;

            // Act
            var result = await handler.Handle(new RemoveCartRequest
            {
                CartDetailsId = cartDetailsId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.ShouldBeOfType<ShoppingCartAPIResponse>();
        }
    }
}