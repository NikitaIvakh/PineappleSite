using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Domain.DTOs;
using Favourite.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourite.Test.Commands;

public sealed class FavouriteProductUpsertRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task FavouriteProductUpsertRequestHandlerTest_Success_Empty()
    {
        // Arrange
        var handler = new FavouriteProductUpsertRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        FavouriteHeaderDto favouriteHeaderDto = new()
        {
            FavouriteHeaderId = 2,
            UserId = "test-userid123"
        };

        List<FavouriteDetailsDto> favouriteDetailsDto =
        [
            new FavouriteDetailsDto()
            {
                FavouriteDetailsId = 2,
                FavouriteHeaderId = 2,
                ProductId = 1,
            }
        ];

        // Act
        var result = await handler.Handle(new FavouriteProductUpsertRequest
            (favouriteDto: new FavouriteDto(favouriteHeaderDto, favouriteDetailsDto)), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(201);
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.SuccessMessage.Should().Be("Продукция успешно добавлена в избранное");
    }

    [Fact]
    public async Task FavouriteProductUpsertRequestHandlerTest_Success_NotEmpty()
    {
        // Arrange
        var handler = new FavouriteProductUpsertRequestHandler(FavouriteHeader, FavouriteDetails, Mapper, MemoryCache);
        FavouriteHeaderDto favouriteHeaderDto = new()
        {
            FavouriteHeaderId = 3,
            UserId = "test-userid12312",
        };

        List<FavouriteDetailsDto> favouriteDetailsDto =
        [
            new FavouriteDetailsDto()
            {
                FavouriteDetailsId = 3,
                FavouriteHeaderId = 3,
                ProductId = 2,
            }
        ];

        // Act
        var result = await handler.Handle(new FavouriteProductUpsertRequest
        (favouriteDto: new FavouriteDto(favouriteHeader: favouriteHeaderDto,
            favouriteDetails: favouriteDetailsDto)), CancellationToken.None);

        // Assert
        result.StatusCode.Should().Be(201);
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.SuccessMessage.Should().Be("Продукция успешно добавлена в избранное");
    }
}