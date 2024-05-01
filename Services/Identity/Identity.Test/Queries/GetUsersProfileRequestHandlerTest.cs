using FluentAssertions;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Test.Common;
using Xunit;

namespace Identity.Test.Queries;

public sealed class GetUsersProfileRequestHandlerTest : TestQueryHandler
{
    [Fact]
    public async Task GetUsersProfileRequestHandlerTest_Success()
    {
        // Arrange
        var handler = new GetUsersProfileRequestHandler(UserRepository, MemoryCache);

        // Act
        var result = await handler.Handle(new GetUsersProfileRequest(), CancellationToken.None);

        // Assert
        result.Count.Should().Be(3);
        result.Data.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.StatusCode.Should().Be(200);
        result.SuccessMessage.Should().Be("Пользователи успешно получены");
    }   
}