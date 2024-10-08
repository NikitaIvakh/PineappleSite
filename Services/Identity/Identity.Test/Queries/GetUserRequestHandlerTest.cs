﻿using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Entities.Users;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Test.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Identity.Test.Queries;

public sealed class GetUserRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetUserDetailsRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetUserRequestHandler(UserManager, MemoryCache);
        const string userId = "3B189631-D179-4200-B77C-B8FC0FD28037";

        // Act
        var result = await handler.Handle(new GetUserRequest(userId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.ErrorMessage.Should().BeNullOrEmpty();
        result.ValidationErrors.Should().BeNullOrEmpty();
        result.Should().BeOfType<Result<GetUserDto>>();

        result.Data!.EmailAddress.Should().Be("user_test_@localhost.com");
        result.Data.FirstName.Should().Be("System");
        result.Data.LastName.Should().Be("User_Test_");
    }

    [Fact]
    public async Task GetUserDetailsRequestHandlerTest_FailOrWrongId()
    {
        // Arrange
        var handler = new GetUserRequestHandler(UserManager, MemoryCache);
        const string userId = "3B189631-D179-4200-B77C-B8FC0FD280372334345555555555";

        // Act
        var result = await handler.Handle(new GetUserRequest(userId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.ErrorMessage.Should().Be("Такого клиента не существует");
        result.StatusCode.Should().Be((int)StatusCode.NotFound);
    }
}