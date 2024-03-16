using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using Identity.Test.Common;
using Xunit;

namespace Identity.Test.Queries
{
    public class GetUserListRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetUserListRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new GetUserListRequestHandler(UserManager, GetUsersLogger, MemoryCache);
            var userId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

            // Act
            var result = await handler.Handle(new GetUserListRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.Count.Should().Be(3);
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
            result.Should().BeOfType<CollectionResult<UserWithRolesDto>>();
        }
    }
}