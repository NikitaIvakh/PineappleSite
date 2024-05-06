using FluentAssertions;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Features.Requests.Handlers;
using Product.Domain.DTOs;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Commands;

public sealed class DeleteProductsRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteProductsDtoRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteProductsRequestHandler(Repository, DeleteProductsValidator, MemoryCache);
        var deleteProducts = new DeleteProductsDto(new List<int> { 1, 2 });

        // Act
        var result = await handler.Handle(new DeleteProductsRequest(deleteProducts), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.SuccessMessage.Should().Be("Продукции успешно удалены");
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteProductsDtoRequestHandlerTest_FailOrWrongIds()
    {
        // Arrange
        var handler = new DeleteProductsRequestHandler(Repository, DeleteProductsValidator, MemoryCache);
        var deleteProducts = new DeleteProductsDto(new List<int>());


        // Act
        var result = await handler.Handle(new DeleteProductsRequest(deleteProducts), CancellationToken.None);

        // Assert
        result.IsSuccess.ShouldBeFalse();
        result.ValidationErrors.Should().NotBeNullOrEmpty();
    }
}