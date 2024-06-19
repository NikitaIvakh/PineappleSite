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

public sealed class CreateUserRequestHandlerTest : TestCommandHandler
{
    [Fact]
    public async Task CreateUserRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new CreateUserRequestHandler(UserManager, CreateUserValidation, MemoryCache);
        var createUserDto = new CreateUserDto
        (
            FirstName: "TestUsename1",
            LastName: "Testuserlastname1",
            Password: "P@ssword1",
            UserName: "testusername",
            EmailAddress: "testemail@gmail.com",
            Roles: UserRoles.User
        );

        // Act
        var result = await handler.Handle(new CreateUserRequest(createUserDto), CancellationToken.None);

        // Assert
        result.Data.Should().NotBeNull();
    }
}