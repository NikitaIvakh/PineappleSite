using Favourite.Application.Features.Handlers.Queries;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Test.Common;
using FluentAssertions;
using Xunit;

namespace Favourite.Test.Queries;

public sealed class GetFavouriteProductsRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetFavouriteProductsRequestHandlerTest_Success()
    {
        //Arrange
        var handler = new GetFavouriteProductsRequestHandler(FavouriteHeader, FavouriteDetails, ProductService,
            Mapper, MemoryCache);
        const string userid = "best-userid1";

        // Act
        var result = await handler.Handle(new GetFavouriteProductsRequest(userId: userid), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result?.Data?.FavouriteHeader.UserId.Should().Be(userid);
    }

    [Fact]
    public async Task GetFavouriteProductsRequestHandlerTest_NewUserId()
    {
        //Arrange
        var handler = new GetFavouriteProductsRequestHandler(FavouriteHeader, FavouriteDetails, ProductService,
            Mapper, MemoryCache);
        const string userid = "best-userid1";

        // Act
        var result = await handler.Handle(new GetFavouriteProductsRequest(userId: userid), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
    }
}