using FluentAssertions;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands;

public sealed class DeleteProductRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteProductDtoRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteProductRequestHandler(Repository, DeleteValidator, MemoryCache);
        const int id = 3;
        var deleteProductDto = new DeleteProductDto(id);

        // Act
        var result = await handler.Handle(new DeleteProductRequest(deleteProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeTrue();
        result.SuccessMessage.Should().Be("Продукт успешно удален");
        result.ValidationErrors.ShouldBeNull();
    }

    [Fact]
    public async Task DeleteProductDtoRequestHandlerTest_FailOrWrong()
    {
        // Arrange
        var handler = new DeleteProductRequestHandler(Repository, DeleteValidator, MemoryCache);
        const int id = 999;
        var deleteProductDto = new DeleteProductDto(id);

        // Act
        var result = await handler.Handle(new DeleteProductRequest(deleteProductDto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Продукт не найден");
        result.ValidationErrors.Should().Equal("Продукт не найден");
    }
}