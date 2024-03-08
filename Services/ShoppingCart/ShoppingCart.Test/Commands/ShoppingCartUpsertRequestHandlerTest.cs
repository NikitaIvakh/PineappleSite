using FluentAssertions;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands
{
    public class ShoppingCartUpsertRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task ShoppingCartUpsertRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new ShoppingCartUpsertRequestHandler(CartHeader, CartDetails, Mapper);
            CartHeaderDto cartHeaderDto = new()
            {
                CartHeaderId = 2,
                UserId = "TestuserId23",
                CouponCode = "5OFF",
                Discount = 10,
                CartTotal = 20,
            };

            CartDetailsDto cartDetailsDto = new()
            {
                CartDetailsId = 2,
                CartHeaderId = 2,
                ProductId = 2,
                Count = 3,
            };

            // Act
            var result = await handler.Handle(new ShoppingCartUpsertRequest
            {
                CartDto = new CartDto
                {
                    CartHeader = cartHeaderDto,
                    CartDetails = new List<CartDetailsDto> { cartDetailsDto },
                },
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ErrorCode.Should().BeNull();
            result.SuccessMessage.Should().Be("Товар успешно добавлен в корзину");
        }

        [Fact]
        public async Task ShoppingCartUpsertRequestHandlerTest_Success_NewUser()
        {
            // Arrange
            var handler = new ShoppingCartUpsertRequestHandler(CartHeader, CartDetails, Mapper);
            CartHeaderDto cartHeaderDto = new()
            {
                CartHeaderId = 3,
                UserId = "TestuserId23234",
                CouponCode = "",
                Discount = 10,
                CartTotal = 20,
            };

            CartDetailsDto cartDetailsDto = new()
            {
                CartDetailsId = 3,
                CartHeaderId = 3,
                ProductId = 3,
                Count = 3,
            };

            // Act
            var result = await handler.Handle(new ShoppingCartUpsertRequest
            {
                CartDto = new CartDto
                {
                    CartHeader = cartHeaderDto,
                    CartDetails = new List<CartDetailsDto> { cartDetailsDto },
                },
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ErrorCode.Should().BeNull();
            result.SuccessMessage.Should().Be("Товар успешно добавлен в корзину");
        }
    }
}