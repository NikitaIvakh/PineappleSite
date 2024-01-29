using Favourites.Application.Features.Commands.Queries;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourites.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetFavouriteProductsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetFavouriteProductsRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetFavouriteProductsRequestHandler(HeaderRepository, DetailsRepository, Mapper, GetFavouriteListLogger, ProductService);
            var userId = "testuserid";

            // Act
            var result = await handler.Handle(new GetFavouriteProductsRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.ValidationErrors.Should().BeNull();
        }

        [Fact]
        public async Task GetFavouriteProductsRequestHandlerTest_NewFavourite()
        {
            // Arrange
            var handler = new GetFavouriteProductsRequestHandler(HeaderRepository, DetailsRepository, Mapper, GetFavouriteListLogger, ProductService);
            var userId = "testuserid123";

            // Act
            var result = await handler.Handle(new GetFavouriteProductsRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.ErrorMessage.Should().BeNull();
            result.IsSuccess.Should().BeTrue();
            result.ValidationErrors.Should().BeNull();
        }
    }
}