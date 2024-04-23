using FluentAssertions;
using Identity.Application.Features.Users.Commands.Handlers;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Domain.DTOs.Identities;
using Identity.Test.Common;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Identity.Test.Commands;

public sealed class DeleteUserRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task DeleteUserRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new DeleteUserRequestHandler(UserRepository, DeleteUser, MemoryCache);
        var deleteUser = new DeleteUserDto
        (
            UserId: "3B189631-D179-4200-B77C-B8FC0FD28037"
        );

        foreach (var entity in Context.ChangeTracker.Entries())
        {
            entity.State = EntityState.Detached;
        }

        // Act
        var result = await handler.Handle(new DeleteUserRequest(deleteUser), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.SuccessMessage.Should().Be("Пользователь успешно удален");
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
    }

    [Fact]
    public async Task DeleteUserRequestHandlerTest_FailOrWrongId()
    {
        // Arrange
        var handler = new DeleteUserRequestHandler(UserRepository, DeleteUser, MemoryCache);
        var deleteUser = new DeleteUserDto
        (
            UserId: "3B189631-D179-4200-B77C-B8FC0FD28011"
        );

        // Act
        var result = await handler.Handle(new DeleteUserRequest(deleteUser), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Такого пользователя не существует");
        result.SuccessMessage.Should().BeNullOrEmpty();
    }
}