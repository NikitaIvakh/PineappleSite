using FluentAssertions;
using Order.Application.Features.Handlers.Requests;
using Order.Application.Features.Requests.Requests;
using Orders.Test.Common;

namespace Orders.Test.Requests
{
    public class GetOrderRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetOrderRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetOrderRequestHandler(OrderHeader, Mapper, MemoryCache);
            var orderId = 1;

            // Act
            var result = await handler.Handle(new GetOrderRequest
            {
                OrderId = orderId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.SuccessMessage.Should().Be("Заказ успешно получен");
            result.ErrorMessage.Should().BeNullOrEmpty();

            result?.Data?.OrderHeaderId.Should().Be(orderId);
            result?.Data?.CouponCode.Should().Be("5off");
            result?.Data?.Name.Should().Be("name");
            result?.Data?.Email.Should().Be("email");
            result?.Data?.PhoneNumber.Should().Be("375445679090");
        }
    }
}