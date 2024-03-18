using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourite.Test.Commands
{
    public class RemoveFavouriteProductRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task RemoveFavoriteRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new RemoveFavouriteProductRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
            var productId = 13;

            // Act
            var result = await handler.Handle(new RemoveFavouriteProductRequest
            {
                ProductId = productId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Внутренняя ошибка сервера");
            result.ErrorCode.Should().Be(500);
        }
    }
}