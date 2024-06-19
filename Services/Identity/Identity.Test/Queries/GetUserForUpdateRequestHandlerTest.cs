using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.Entities.Users;
using Identity.Test.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Identity.Test.Queries;

public sealed class GetUserForUpdateRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetUserForUpdateRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetUserForUpdateRequestHandler(UserManager, MemoryCache);
        const string userId = "3B189631-D179-4200-B77C-B8FC0FD28037";
        const string password = "P@ssword1";

        // Act
        var result = await handler.Handle(new GetUserForUpdateRequest(userId, password), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.SuccessMessage.Should().Be("Клиент успешно получен");
        result.Data.Should().NotBeNull();
    }
    
    [Fact]
    public async Task GetUserForUpdateRequestHandlerTest_FailOrWrong_UserId()
    {
        // Arrange
        var handler = new GetUserForUpdateRequestHandler(UserManager, MemoryCache);
        const string userId = "3B189631-D179-4200-B77C-B8FC0FD28099";
        const string password = "P@ssword1";

        // Act
        var result = await handler.Handle(new GetUserForUpdateRequest(userId, password), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.StatusCode.Should().Be(404);
        result.ErrorMessage.Should().Be("Такого клиента не существует");
        result.ValidationErrors.Should().Equal("Такого клиента не существует");
    }
}