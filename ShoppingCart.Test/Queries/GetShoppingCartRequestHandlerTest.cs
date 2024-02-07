using FluentAssertions;
using ShoppingCart.Application.Features.Handlers.Queries;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Test.Common;
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
            var handler = new GetShoppingCartRequestHandler(CartHeader, CartDetails, ProductService, CouponService, Mapper);
            var userId = "TestuserId23";

            // Act
            var result = await handler.Handle(new GetShoppingCartRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result?.Data?.CartHeader.CouponCode.Should().Be("5OFF");
        }

        [Fact]
        public async Task GetShoppingCartRequestHandlerTest_NewUserId()
        {
            // Arrange
            var handler = new GetShoppingCartRequestHandler(CartHeader, CartDetails, ProductService, CouponService, Mapper);
            var userId = "newuserid";

            // Act
            var result = await handler.Handle(new GetShoppingCartRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
            result.SuccessMessage.Should().Be("Ваша корзина пуста! Добавьте товары");
        }
    }
}