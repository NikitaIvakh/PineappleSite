using Favourite.Application.Features.Handlers.Queries;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourite.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetFavouriteFroductsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetFavouriteFroductsRequestHandlerTest_Success()
        {
            //Arrange
            var handler = new GetFavouriteFroductsRequestHandler(FavouriteHeader, FavouriteDetails, ProductService, Mapper, MemoryCache);
            var userid = "bestuserid1";

            // Act
            var result = await handler.Handle(new GetFavouriteFroductsRequest
            {
                UserId = userid,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result?.Data?.FavouriteHeader.UserId.Should().Be(userid);
        }

        [Fact]
        public async Task GetFavouriteFroductsRequestHandlerTest_NewUserId()
        {
            //Arrange
            var handler = new GetFavouriteFroductsRequestHandler(FavouriteHeader, FavouriteDetails, ProductService, Mapper, MemoryCache);
            var userid = "bestuserid12";

            // Act
            var result = await handler.Handle(new GetFavouriteFroductsRequest
            {
                UserId = userid,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
        }
    }
}