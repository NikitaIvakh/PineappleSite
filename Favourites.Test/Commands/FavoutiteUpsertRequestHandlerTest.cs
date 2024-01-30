using Favourites.Application.Features.Commands.Handlers;
using Favourites.Application.Features.Requests.Handlers;
using Favourites.Domain.DTOs;
using Favourites.Domain.ResultFavourites;
using Favourites.Test.Common;
using Shouldly;
using Xunit;

namespace Favourites.Test.Commands
{
    public class FavoutiteUpsertRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task FavoutiteUpsertRequestHandlerTest_Success_Empty()
        {
            // Arrange
            var handler = new FavoutiteUpsertRequestHandler(HeaderRepository, DetailsRepository, Mapper, UpsertLogger);
            FavouritesDetailsDto favouriteDetails = new()
            {
                FavouritesDetailsId = 2,
                FavouritesHeaderId = 2,
                ProductId = 3,
            };

            var favouritesDto = new FavouritesDto
            {
                FavoutiteHeader = new FavouritesHeaderDto
                {
                    FavouritesHeaderId = 2,
                    UserId = "tetsuserid31234",
                },

                FavouritesDetails = new CollectionResult<FavouritesDetailsDto> { Data = new List<FavouritesDetailsDto> { favouriteDetails } }
            };

            // Act
            var result = await handler.Handle(new FavoutiteUpsertRequest
            {
                Favourites = favouritesDto,
            }, CancellationToken.None);

            // Assert
            result.SuccessMessage.ShouldNotBeNull();
        }

        [Fact]
        public async Task FavoutiteUpsertRequestHandlerTest_Success_NotEmpty()
        {
            // Arrange
            var handler = new FavoutiteUpsertRequestHandler(HeaderRepository, DetailsRepository, Mapper, UpsertLogger);
            FavouritesDetailsDto favouriteDetails = new()
            {
                FavouritesDetailsId = 2,
                FavouritesHeaderId = 2,
                ProductId = 3,
            };

            var favouritesDto = new FavouritesDto
            {
                FavoutiteHeader = new FavouritesHeaderDto
                {
                    FavouritesHeaderId = 2,
                    UserId = "tetsuserid3",
                },

                FavouritesDetails = new CollectionResult<FavouritesDetailsDto> { Data = new List<FavouritesDetailsDto> { favouriteDetails } }
            };

            // Act
            var result = await handler.Handle(new FavoutiteUpsertRequest
            {
                Favourites = favouritesDto,
            }, CancellationToken.None);

            // Assert
            result.SuccessMessage.ShouldBe("Продукт успешно добавлен в избранное");
        }
    }
}