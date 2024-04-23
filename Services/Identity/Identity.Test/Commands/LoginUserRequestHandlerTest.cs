// using FluentAssertions;
// using Identity.Application.Features.Identities.Commands.Commands;
// using Identity.Application.Features.Identities.Requests.Commands;
// using Identity.Domain.DTOs.Authentications;
// using Identity.Test.Common;
// using Xunit;
//
// namespace Identity.Test.Commands;
//
// public sealed class LoginUserRequestHandlerTest : TestCommandHandler
// {
//     [Fact]
//     public async Task LoginUserRequestHandlerTest_Success()
//     {
//         // Arrange
//         var handler = new LoginUserRequestHandler(UserRepository, AuthRequest, TokenService, Configuration);
//         var authRequestDto = new AuthRequestDto
//         (
//             EmailAddress: "admin@localhost.com",
//             Password: "P@ssword1"
//         );
//
//         // Act
//         var result = await handler.Handle(new LoginUserRequest(authRequestDto), CancellationToken.None);
//
//         // Assert
//         result.IsSuccess.Should().BeTrue();
//         result.Data.Should().NotBeNullOrEmpty();
//         result.SuccessMessage.Should().Be("");
//     }
// }