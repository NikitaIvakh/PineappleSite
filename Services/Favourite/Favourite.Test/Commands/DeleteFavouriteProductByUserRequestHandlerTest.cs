using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Domain.DTOs;
using Favourite.Test.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Favourite.Test.Commands;

public class DeleteFavouriteProductByUserRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteFavouriteProductByUserRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteFavouriteProductByUserRequestHandler(FavouriteHeader, FavouriteDetails,
            DeleteByUserValidator, MemoryCache);
        const string userId = "best-userid1";
        const int productId = 2;
        var deleteDFavouriteProductByUserDto = new DeleteFavouriteProductByUserDto(productId, userId);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductByUserRequest(deleteDFavouriteProductByUserDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Продукт успешно удален");
    }
    
    [Fact]
    public async Task DeleteFavouriteProductByUserRequestHandlerTest_FailOrWrong_ProductId()
    {
        // Arrange
        var handler = new DeleteFavouriteProductByUserRequestHandler(FavouriteHeader, FavouriteDetails,
            DeleteByUserValidator, MemoryCache);
        const string userId = "best-userid1";
        const int productId = 22;
        var deleteDFavouriteProductByUserDto = new DeleteFavouriteProductByUserDto(productId, userId);

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductByUserRequest(deleteDFavouriteProductByUserDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Продукт не найден");
        result.ValidationErrors.Should().Equal("Продукт не найден");
    }
    
    [Fact]
    public async Task DeleteFavouriteProductByUserRequestHandlerTest_FailOrWrong_UserId()
    {
        // Arrange
        var handler = new DeleteFavouriteProductByUserRequestHandler(FavouriteHeader, FavouriteDetails,
            DeleteByUserValidator, MemoryCache);
        const string userId = "best-userid112334455433";
        const int productId = 22;
        var deleteDFavouriteProductByUserDto = new DeleteFavouriteProductByUserDto(productId, userId);

        // Act
        var result = await handler.Handle(new DeleteFavouriteProductByUserRequest(deleteDFavouriteProductByUserDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Продукт не найден");
        result.ValidationErrors.Should().Equal("Продукт не найден");
    }
}