using FluentAssertions;
using Order.Application.Features.Handlers.Requests;
using Orders.Test.Common;
using Order.Application.Features.Requests.Requests;

namespace Orders.Test.Requests;

public sealed class GetOrdersRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetOrderListRequestHandlerTest_Fail()
    {
        // Arrange
        var handler = new GetOrdersRequestHandler(OrderHeader, Mapper, MemoryCache, UserService);
        const string userId = "testuser5t654";

        // Act
        var result = await handler.Handle(new GetOrdersRequest(userId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.SuccessMessage.Should().Be("Список заказов");
    }
}