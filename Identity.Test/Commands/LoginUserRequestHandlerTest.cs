//using FluentAssertions;
//using Identity.Application.Features.Identities.Commands.Commands;
//using Identity.Application.Features.Identities.Requests.Commands;
//using Identity.Domain.DTOs.Authentications;
//using Identity.Test.Common;
//using Xunit;

//namespace Identity.Test.Commands
//{
//    public class LoginUserRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task LoginUserRequestHandlerTest_Success()
//        {
//            // Arrange
//            var handler = new LoginUserRequestHandler(UserManager, SignInManager, JwtSettings, AuthRequest, TokenProvider, HttpContextAccessor, AuthUserLogger);
//            var authRequestDto = new AuthRequestDto
//            {
//                Email = "admin@localhost.com",
//                Password = "P@ssword1",
//            };

//            // Act
//            var result = await handler.Handle(new LoginUserRequest
//            {
//                AuthRequest = authRequestDto,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.Should().BeTrue();
//        }
//    }
//}