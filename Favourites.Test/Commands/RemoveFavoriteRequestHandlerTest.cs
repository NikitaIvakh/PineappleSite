using Favourites.Application.Features.Commands.Handlers;
using Favourites.Application.Features.Requests.Handlers;
using Favourites.Test.Common;
using FluentAssertions;
using Shouldly;
using Xunit;

namespace Favourites.Test.Commands
{
    public class RemoveFavoriteRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task RemoveFavoriteRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new RemoveFavoriteRequestHandler(HeaderRepository, DetailsRepository, RemoveLogger, null);
            var favouriteDetailId = 3;

            // Act
            var result = await handler.Handle(new RemoveFavoriteRequest
            {
                FavouriteDetailId = favouriteDetailId,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.ShouldBeNull();
        }

        [Fact]
        public async Task RemoveFavoriteRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new RemoveFavoriteRequestHandler(HeaderRepository, DetailsRepository, RemoveLogger, null);
            var favouriteDetailId = 13;

            // Act
            var result = await handler.Handle(new RemoveFavoriteRequest
            {
                FavouriteDetailId = favouriteDetailId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.ShouldBe("Избранный продукт не найден");
            result.ErrorCode.ShouldBe(404);
        }
    }
}