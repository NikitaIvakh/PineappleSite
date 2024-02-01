using FluentAssertions;
using Identity.Application.Features.Identities.Commands.Commands;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Domain.DTOs.Identities;
using Identity.Test.Common;
using Xunit;

namespace Identity.Test.Commands
{
    public class DeleteUserListRequestHandlerTest : TestCommandHandler
    {
        [Fact]
        public async Task DeleteUserListRequestHandlerTest_Success()
        {
            // Arrange
            var handler = new DeleteUserListRequestHandler(UserManager, DeleteUsers, DeleteUserListLogger, Mapper);
            var deleteUserList = new DeleteUserListDto()
            {
                UserIds = ["8e445865-a24d-4543-a6c6-9443d048cdb9", "9e224968-33e4-4652-b7b7-8574d048cdb9"],
            };

            // Act
            var result = await handler.Handle(new DeleteUserListRequest
            {
                DeleteUserList = deleteUserList,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Count.Should().Be(2);
            result.SuccessMessage.Should().Be("Пользователи успешно удалены");
            result.ErrorMessage.Should().BeNullOrEmpty();
            result.ValidationErrors.Should().BeNullOrEmpty();
        }
    }
}