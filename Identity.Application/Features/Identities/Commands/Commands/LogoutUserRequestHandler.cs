using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Domain.Enum;
using Identity.Domain.Interface;
using Identity.Domain.ResultIdentity;
using MediatR;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LogoutUserRequestHandler(ITokenProvider tokenProvider, ILogger logger) : IRequestHandler<LogoutUserRequest, Result<bool>>
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly ILogger _logger = logger.ForContext<LogoutUserRequestHandler>();

        public async Task<Result<bool>> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                _tokenProvider.ClearToken();

                return new Result<bool>
                {
                    Data = true,
                    SuccessMessage = "Вы успешно вышли из аккаунта",
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<bool>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}