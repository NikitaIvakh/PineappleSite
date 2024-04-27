// using FluentAssertions;
// using Microsoft.EntityFrameworkCore;
// using Order.Application.Features.Handlers.Commands;
// using Order.Application.Features.Requests.Commands;
// using Order.Application.Utility;
// using Orders.Test.Common;
//
// namespace Orders.Test.Commands;
//
// public class UpdateOrderStatusRequestHandlerTest : TestCommandsHandler
// {
//     [Fact]
//     public async Task UpdateOrderStatusRequestHandler_Success()
//     {
//         // Arrange
//         var handler = new UpdateOrderStatusRequestHandler(OrderHeader, Mapper, MemoryCache);
//         const int orderId = 1;
//         const string orderStatus = StaticDetails.StatusApproved;
//
//         foreach (var entity in Context.ChangeTracker.Entries())
//         {
//             entity.State = EntityState.Detached;
//         }
//
//         //Act
//         var result = await handler.Handle(new UpdateOrderStatusRequest()
//         {
//             OrderHeaderId = orderId,
//             NewStatus = orderStatus,
//         }, CancellationToken.None);
//
//         // Assert
//         result.SuccessCode.Should().Be(203);
//         result.SuccessMessage.Should().Be("Статус заказа успешно обновлен");
//         result.ErrorCode.Should().BeNull();
//         result.ErrorMessage.Should().BeNull();
//         result.ValidationErrors.Should().BeNullOrEmpty();
//     }
// }