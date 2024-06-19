using FluentAssertions;
using Identity.Application.Features.Users.Commands.Handlers;
using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Test.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Identity.Test.Commands;

public sealed class UpdateUserRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task UpdateUserRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new UpdateUserRequestHandler(UserManager, UpdateUserValidator, MemoryCache);
        var updateUserDto = new UpdateUserDto
        (
            Id: "3B189631-D179-4200-B77C-B8FC0FD28037",
            FirstName: "test",
            LastName: "test",
            UserName: "test",
            EmailAddress: "test@gmail.com",
            UserRoles: UserRoles.User
        );

        // Act
        var result = await handler.Handle(new UpdateUserRequest(updateUserDto), CancellationToken.None);

        // Assert
        result.Data.Should().NotBeNull();
    }
}