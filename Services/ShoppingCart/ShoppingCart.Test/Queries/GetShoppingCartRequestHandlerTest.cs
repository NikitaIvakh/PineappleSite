﻿using FluentAssertions;
using ShoppingCart.Application.Features.Handlers.Queries;
using ShoppingCart.Application.Features.Requests.Queries;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Queries;

public sealed class GetShoppingCartRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetShoppingCartRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetShoppingCartRequestHandler(CartHeader, CartDetails, ProductService, CouponService,
            Mapper, MemoryCache);
        const string userId = "TestuserId23";

        // Act
        var result = await handler.Handle(new GetShoppingCartRequest(userId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result?.Data?.CartHeader.CouponCode.Should().Be("5OFF");
    }

    [Fact]
    public async Task GetShoppingCartRequestHandlerTest_NewUserId()
    {
        // Arrange
        var handler = new GetShoppingCartRequestHandler(CartHeader, CartDetails, ProductService, CouponService,
            Mapper, MemoryCache);
        const string userId = "newuserid";

        // Act
        var result = await handler.Handle(new GetShoppingCartRequest(userId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.SuccessMessage.Should().Be("Ваша корзина пуста! Добавьте товары");
    }
}