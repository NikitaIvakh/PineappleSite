using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Favourite.Test.Commands;

public sealed class DeleteFavouriteProductRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task RemoveFavoriteRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteFavouriteProductRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        const int productId = 2;

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductRequest(productId: productId),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.SuccessMessage.Should().Be("Продукт успешно удален");
        result.StatusCode.Should().Be(205);
    }

    [Fact]
    public async Task RemoveFavoriteRequestHandlerTest_FailOrWrongId()
    {
        // Arrange
        var handler = new DeleteFavouriteProductRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        const int productId = 13;

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductRequest(productId: productId),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Продукт не найден");
        result.StatusCode.Should().Be(404);
    }
}