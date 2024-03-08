using Favourite.Application.Features.Handlers.Commands;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Domain.DTOs;
using Favourite.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourite.Test.Commands
{
    public class FavouriteProductUpsertRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task FavouriteProductUpsertRequestHandlerTest_Success_Empty()
        {
            // Arrange
            var handler = new FavouriteProductUpsertRequestHandler(FavouriteHeader, FavouriteDetails, Mapper);
            FavouriteHeaderDto favouriteHeaderDto = new()
            {
                FavouriteHeaderId = 2,
                UserId = "testuserid123"
            };

            FavouriteDetailsDto favouriteDetailsDto = new()
            {
                FavouriteDetailsId = 2,
                FavouriteHeaderId = 2,
                ProductId = 1,
            };

            // Act
            var result = await handler.Handle(new FavouriteProductUpsertRequest
            {
                FavouriteDto = new FavouriteDto
                {
                    FavouriteHeader = favouriteHeaderDto,
                    FavouriteDetails = new List<FavouriteDetailsDto> { favouriteDetailsDto },
                }
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ErrorCode.Should().BeNull();
            result.SuccessMessage.Should().Be("Продукт успешно добавлен в избранное");
        }

        [Fact]
        public async Task FavouriteProductUpsertRequestHandlerTest_Success_NotEmpty()
        {
            // Arrange
            var handler = new FavouriteProductUpsertRequestHandler(FavouriteHeader, FavouriteDetails, Mapper);
            FavouriteHeaderDto favouriteHeaderDto = new()
            {
                FavouriteHeaderId = 3,
                UserId = "testuserid12312",
            };

            FavouriteDetailsDto favouriteDetailsDto = new()
            {
                FavouriteDetailsId = 3,
                FavouriteHeaderId = 3,
                ProductId = 2,
            };

            // Act
            var result = await handler.Handle(new FavouriteProductUpsertRequest
            {
                FavouriteDto = new FavouriteDto
                {
                    FavouriteHeader = favouriteHeaderDto,
                    FavouriteDetails = new List<FavouriteDetailsDto> { favouriteDetailsDto },
                }
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ErrorCode.Should().BeNull();
            result.SuccessMessage.Should().Be("Продукт успешно добавлен в избранное");
        }
    }
}