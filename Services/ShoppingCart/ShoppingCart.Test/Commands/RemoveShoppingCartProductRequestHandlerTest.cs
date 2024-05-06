using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class RemoveShoppingCartProductRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task RemoveShoppingCartDetailsRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new RemoveShoppingCartProductRequestHandler(CartHeader, CartDetails, DeleteValidator, MemoryCache);
        const int productId = 3;
        var deleteProductDto = new DeleteProductDto(productId);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new RemoveShoppingCartProductRequest(deleteProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Продукция успешно удалена");
    }
    
    [Fact]
    public async Task RemoveShoppingCartDetailsRequestHandlerTest_FailOrWrong_ProductId()
    {
        // Arrange
        var handler = new RemoveShoppingCartProductRequestHandler(CartHeader, CartDetails, DeleteValidator, MemoryCache);
        const int productId = 32;
        var deleteProductDto = new DeleteProductDto(productId);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new RemoveShoppingCartProductRequest(deleteProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Продукции не найдены");
        result.ValidationErrors.Should().Equal("Продукции не найдены");
    }
}