using FluentAssertions;
using Identity.Application.Features.Users.Commands.Handlers;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Test.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Identity.Test.Commands;

public class DeleteUserListRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteUserListRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteUsersRequestHandler(UserManager, DeleteUsers, MemoryCache);
        var deleteUserList = new DeleteUsersDto(new List<string>()
            { "8e445865-a24d-4543-a6c6-9443d048cdb9", "9e224968-33e4-4652-b7b7-8574d048cdb9" });

        // Act
        var result = await handler.Handle(new DeleteUsersRequest(deleteUserList), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Count.Should().Be(2);
        result.SuccessMessage.Should().Be("Клиенты успешно удалены");
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteUserListRequestHandlerTest_Success_FailOrWrongIds()
    {
        // Arrange
        var handler = new DeleteUsersRequestHandler(UserManager, DeleteUsers, MemoryCache);
        var deleteUserList = new DeleteUsersDto(new List<string>()
            { "8e445865-a24d-4543-a6c6-9443d048cdb1", "9e224968-33e4-4652-b7b7-8574d048cdb2" });

        // Act
        var result = await handler.Handle(new DeleteUsersRequest(deleteUserList), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Клиенты не найдены");
        result.SuccessMessage.Should().BeNullOrEmpty();
    }
}