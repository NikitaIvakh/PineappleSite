using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Handlers.Commands;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Test.Common;
using Xunit;

namespace ShoppingCart.Test.Commands;

public sealed class DeleteCartProductByUserRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteCartProductByUserRequestHandlerTest_Success()
    {
        // Arrange
        var hander =
            new DeleteCartProductByUserRequestHandler(CartHeader, CartDetails, DeleteByUserValidator, MemoryCache);
        const int productId = 3;
        const string userId = "TestuserId23";
        var deleteProductByUserDto = new DeleteProductByUserDto(productId, userId);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await hander.Handle(new DeleteCartProductByUserRequest(deleteProductByUserDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(205);
        result.SuccessMessage.Should().Be("Продукт успешно удален");
    }

    [Fact]
    public async Task DeleteCartProductByUserRequestHandlerTest_FailOrWrong_ProductId()
    {
        // Arrange
        var hander =
            new DeleteCartProductByUserRequestHandler(CartHeader, CartDetails, DeleteByUserValidator, MemoryCache);
        const int productId = 31;
        const string userId = "TestuserId23";
        var deleteProductByUserDto = new DeleteProductByUserDto(productId, userId);

        // Act
        var result = await hander.Handle(new DeleteCartProductByUserRequest(deleteProductByUserDto),
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Продукт не найден");
        result.ValidationErrors.Should().Equal("Продукт не найден");
    }
}