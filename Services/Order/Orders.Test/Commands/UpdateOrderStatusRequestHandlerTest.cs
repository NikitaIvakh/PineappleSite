using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Order.Application.Features.Handlers.Commands;
using Order.Application.Features.Requests.Commands;
using Order.Application.Utility;
using Order.Domain.DTOs;
using Orders.Test.Common;

namespace Orders.Test.Commands;

public sealed class UpdateOrderStatusRequestHandlerTest : TestCommandsHandler
{
    [Fact]
    public async Task UpdateOrderStatusRequestHandler_Success()
    {
        // Arrange
        var handler = new UpdateOrderStatusRequestHandler(OrderHeader, Mapper, MemoryCache);
        const int orderId = 1;
        const string orderStatus = StaticDetails.StatusApproved;
        var updateOrderStatusdto = new UpdateOrderStatusDto(orderId, orderStatus);

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        //Act
        var result = await handler.Handle(new UpdateOrderStatusRequest(updateOrderStatusdto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(203);
        result.SuccessMessage.Should().Be("Статус заказа успешно обновлен");
    }
    
    [Fact]
    public async Task UpdateOrderStatusRequestHandler_Success_FailOrWrong_OrderId()
    {
        // Arrange
        var handler = new UpdateOrderStatusRequestHandler(OrderHeader, Mapper, MemoryCache);
        const int orderId = 1999;
        const string orderStatus = StaticDetails.StatusApproved;
        var updateOrderStatusdto = new UpdateOrderStatusDto(orderId, orderStatus);


        //Act
        var result = await handler.Handle(new UpdateOrderStatusRequest(updateOrderStatusdto), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Заголовок заказов не найден");
        result.ValidationErrors.Should().Equal("Заголовок заказов не найден");
    }
}