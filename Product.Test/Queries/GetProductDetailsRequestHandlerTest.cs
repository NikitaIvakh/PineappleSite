using FluentAssertions;
using Product.Application.Features.Commands.Queries;
using Product.Application.Features.Requests.Queries;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Queries
{
    public class GetProductDetailsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetProductDetailsRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetProductDetailsRequestHandler(Repository, Logger, Mapper);
            var producId = 4;

            // Act
            var result = await handler.Handle(new GetProductDetailsRequest
            {
                Id = producId,
            }, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Data.Name.ShouldBe("Test 1");
            result.Data.Description.ShouldBe("Test product 1");
            result.Data.Price.ShouldBe(10);
        }

        [Fact]
        public async Task GetProductDetailsRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new GetProductDetailsRequestHandler(Repository, Logger, Mapper);
            var productId = 999;

            // Act && Assert
            var result = await handler.Handle(new GetProductDetailsRequest
            { 
                Id = productId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Продукт не найден");
        }
    }
}