﻿using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.Entities.Users;
using Identity.Test.Common;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Identity.Test.Queries;

public sealed class GetUsersRequestHandlerTest : TestQueryHandler
{ 
    [Fact]
    public async Task GetUserListRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetUsersRequestHandler(UserManager, MemoryCache);

        // Act
        var result = await handler.Handle(new GetUsersRequest(), CancellationToken.None);

        // Assert
        result.Count.Should().Be(3);
        result.IsSuccess.Should().BeTrue();
    }
}