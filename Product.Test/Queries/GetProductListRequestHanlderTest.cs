using FluentAssertions;
using Product.Application.Features.Commands.Queries;
using Product.Application.Features.Requests.Queries;
using Product.Test.Common;
using Shouldly;
using Xunit;

namespace Product.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetProductListRequestHanlderTest : TestQueryHandler
    {
        [Fact]
        public async Task GetProductListRequestHanlderTest_Success()
        {
            // Arrange
            var handler = new GetProductListRequestHandler(Repository, Logger);

            // Act
            var result = await handler.Handle(new GetProductListRequest(), CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Count.Should().Be(6);
        }
    }
}