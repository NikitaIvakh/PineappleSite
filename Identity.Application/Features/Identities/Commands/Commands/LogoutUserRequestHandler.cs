using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Application.Services.IServices;
using MediatR;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LogoutUserRequestHandler(ITokenProvider tokenProvider) : IRequestHandler<LogoutUserRequest, BaseIdentityResponse<bool>>
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;

        public async Task<BaseIdentityResponse<bool>> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<bool>();

            try
            {
                _tokenProvider.ClearToken();

                return new BaseIdentityResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Успешный выход из аккаунта",
                    Data = true
                };
            }

            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Message = exception.Message;
            }

            return response;
        }
    }
}