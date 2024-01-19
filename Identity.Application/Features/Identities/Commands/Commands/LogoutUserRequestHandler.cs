using Identity.Application.DTOs.Identities;
using Identity.Application.DTOs.Validators;
using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Application.Services.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LogoutUserRequestHandler(ITokenProvider tokenProvider, IHttpContextAccessor httpContextAccessor, ILogoutUserDtoValidator validator) : IRequestHandler<LogoutUserRequest, BaseIdentityResponse<LogoutUserDto>>
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogoutUserDtoValidator _logoutUserValidator = validator;

        public async Task<BaseIdentityResponse<LogoutUserDto>> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<LogoutUserDto>();

            try
            {
                var validator = await _logoutUserValidator.ValidateAsync(request.LogoutUser, cancellationToken);

                if (!validator.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка выхода из аккаунта";
                    response.ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
                    _tokenProvider.ClearToken();

                    var logoutUserDto = new LogoutUserDto
                    {
                        UserId = request.LogoutUser.UserId,
                    };

                    response.IsSuccess = true;
                    response.Message = "Вы успешно вышли из аккаунта";
                    response.Data = logoutUserDto;

                    return response;
                }
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