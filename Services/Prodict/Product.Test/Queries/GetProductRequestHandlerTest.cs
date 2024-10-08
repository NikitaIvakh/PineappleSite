﻿using FluentAssertions;
using Product.Application.Features.Commands.Queries;
using Product.Application.Features.Requests.Queries;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Queries;

public sealed class GetProductRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetProductDetailsRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetProductRequestHandler(Repository, MemoryCache, Mapper);
        const int producId = 4;

        // Act
        var result = await handler.Handle(new GetProductRequest(producId), CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Data!.Name.Should().Be("Test 1");
        result.Data.Description.Should().Be("Test product 1");
        result.Data.Price.Should().Be(10);
    }

    [Fact]
    public async Task GetProductDetailsRequestHandlerTest_FailOrWrongId()
    {
        // Arrange
        var handler = new GetProductRequestHandler(Repository, MemoryCache, Mapper);
        const int productId = 999;

        // Act && Assert
        var result = await handler.Handle(new GetProductRequest(productId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Продукт не найден");
    }
}