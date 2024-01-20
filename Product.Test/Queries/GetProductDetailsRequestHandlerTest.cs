using Product.Application.Features.Commands.Queries;
using Product.Application.Features.Requests.Queries;
using Product.Test.Common;
using Should;
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
            var handler = new GetProductDetailsRequestHandler(Context, Mapper);
            var producId = 4;

            // Act
            var result = await handler.Handle(new GetProductDetailsRequest
            {
                Id = producId,
            }, CancellationToken.None);

            // Assert
            result.ShouldNotBeNull();
            result.Name.ShouldBe("Test 1");
            result.Description.ShouldBe("Test product 1");
            result.Price.ShouldBe(10);
        }
    }
}