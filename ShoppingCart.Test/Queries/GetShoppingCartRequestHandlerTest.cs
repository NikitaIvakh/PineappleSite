using ShoppingCart.Application.Features.Commands.Queries;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Test.Common;
using Shouldly;
using Xunit;

namespace ShoppingCart.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetShoppingCartRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetShoppingCartRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetShoppingCartRequestHandler(Context, Context, Mapper, ProductService, CouponService);
            var userId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

            // Act
            var result = await handler.Handle(new GetShoppingCartRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.ShouldBeTrue();
            result.Id.ShouldBe(1);
            result.ValidationErrors.ShouldBeNull();
        }
    }
}