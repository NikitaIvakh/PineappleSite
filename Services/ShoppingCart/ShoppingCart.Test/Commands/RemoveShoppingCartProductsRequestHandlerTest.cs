using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class RemoveShoppingCartProductsRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task RemoveShoppingCartProductsRequestHandlerTest_Success()
    {
        // Arrange
        var handler =
            new RemoveShoppingCartProductsRequestHandler(CartHeader, CartDetails, DeleteProductsValidator, MemoryCache);
        var productIds = new List<int> { 3 };
        var deleteProductsDto = new DeleteProductsDto([..productIds]);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new RemoveShoppingCartProductsRequest(deleteProductsDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Продукции успешно удалены");
    }
    
    [Fact]
    public async Task RemoveShoppingCartProductsRequestHandlerTest_FailOrWrong_ProductIds()
    {
        // Arrange
        var handler =
            new RemoveShoppingCartProductsRequestHandler(CartHeader, CartDetails, DeleteProductsValidator, MemoryCache);
        var productIds = new List<int> { 33 };
        var deleteProductsDto = new DeleteProductsDto([..productIds]);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new RemoveShoppingCartProductsRequest(deleteProductsDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Продукции не найдены");
        result.ValidationErrors.Should().Equal("Продукции не найдены");
    }
}