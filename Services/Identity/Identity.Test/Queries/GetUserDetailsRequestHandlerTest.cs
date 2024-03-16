﻿using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Enum;
using Identity.Domain.ResultIdentity;
using Identity.Test.Common;
using Xunit;

namespace Identity.Test.Queries
{
    public class GetUserDetailsRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetUserDetailsRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetUserDetailsRequestHandler(UserManager, GetUserLogger, Mapper, MemoryCache);
            var userId = "3B189631-D179-4200-B77C-B8FC0FD28037";

            // Act
            var result = await handler.Handle(new GetUserDetailsRequest
            {
                Id = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
            result.Should().BeOfType<Result<UserWithRolesDto>>();

            result.Data.User.Email.Should().Be("user_test_@localhost.com");
            result.Data.User.FirstName.Should().Be("System");
            result.Data.User.LastName.Should().Be("User_Test_");
            result.Data.User.Description.Should().Be("Test_Test");
            result.Data.User.Age.Should().Be(24);
        }

        [Fact]
        public async Task GetUserDetailsRequestHandlerTest_FailOrWrongId()
        {
            // Arrange
            var handler = new GetUserDetailsRequestHandler(UserManager, GetUserLogger, Mapper, MemoryCache);
            var userId = "3B189631-D179-4200-B77C-B8FC0FD280372334345555555555";

            // Act
            var result = await handler.Handle(new GetUserDetailsRequest
            {
                Id = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Такого пользователя не существует");
            result.ErrorCode.Should().Be((int)ErrorCodes.UserNotFound);
        }
    }
}