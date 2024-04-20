using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Domain.DTOs;
using Favourite.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Favourite.Test.Commands;

public sealed class DeleteFavouriteProductsRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteFavouriteProductsRequestHandler_Success()
    {
        // Arrange
        var handler = new DeleteFavouriteProductsRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        var productIds = new List<int>() { 2 };
        var deleteFavouriteProductsDto = new DeleteFavouriteProductsDto(productIds);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductsRequest(deleteFavouriteProductsDto),
            CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(205);
    }

    [Fact]
    public async Task DeleteFavouriteProductsRequestHandler_FailOrWrong_ProductIds()
    {
        // Arrange
        var handler = new DeleteFavouriteProductsRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        var productIds = new List<int>() { 333 };
        var deleteFavouriteProductsDto = new DeleteFavouriteProductsDto(productIds);

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductsRequest(deleteFavouriteProductsDto),
            CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Продукты не найдены");
    }
}