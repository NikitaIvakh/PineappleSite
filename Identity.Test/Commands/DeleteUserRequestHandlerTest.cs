//using FluentAssertions;
//using Identity.Application.Features.Identities.Commands.Commands;
//using Identity.Application.Features.Identities.Requests.Commands;
//using Identity.Domain.DTOs.Identities;
//using Identity.Test.Common;
//using Xunit;

//namespace Identity.Test.Commands
//{
//    public class DeleteUserRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task DeleteUserRequestHandlerTest_Success()
//        {
//            // Arrange
//            var handler = new DeleteUserRequestHandler(UserManager, DeleteUser, DeleteUserLogger);
//            var deleteUser = new DeleteUserDto
//            {
//                Id = "3B189631-D179-4200-B77C-B8FC0FD28037"
//            };

//            // Act
//            var result = await handler.Handle(new DeleteUserRequest
//            {
//                DeleteUser = deleteUser
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.Should().BeTrue();
//            result.SuccessMessage.Should().Be("Пользователь успешно удален");
//            result.ErrorMessage.Should().BeNullOrEmpty();
//            result.ValidationErrors.Should().BeNullOrEmpty();
//        }

//        [Fact]
//        public async Task DeleteUserRequestHandlerTest_FailOrWrongId()
//        {
//            // Arrange
//            var handler = new DeleteUserRequestHandler(UserManager, DeleteUser, DeleteUserLogger);
//            var deleteUser = new DeleteUserDto
//            {
//                Id = "3B189631-D179-123"
//            };

//            // Act
//            var result = await handler.Handle(new DeleteUserRequest
//            {
//                DeleteUser = deleteUser,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.Should().BeFalse();
//            result.ErrorMessage.Should().Be("Такого пользователя не существует");
//            result.SuccessMessage.Should().BeNullOrEmpty();
//        }
//    }
//}