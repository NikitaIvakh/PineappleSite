﻿using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Resources;
using Identity.Application.Validators;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.Enum;
using Identity.Domain.Interface;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LogoutUserRequestHandler(ITokenProvider tokenProvider, IHttpContextAccessor httpContextAccessor, ILogoutUserDtoValidator validator, ILogger logger) : IRequestHandler<LogoutUserRequest, Result<LogoutUserDto>>
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ILogoutUserDtoValidator _logoutUserValidator = validator;
        private readonly ILogger _logger = logger.ForContext<LogoutUserRequestHandler>();

        public async Task<Result<LogoutUserDto>> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _logoutUserValidator.ValidateAsync(request.LogoutUser, cancellationToken);

                if (!validator.IsValid)
                {
                    return new Result<LogoutUserDto>
                    {
                        ErrorMessage = ErrorMessage.ErrorLoggingOutOfTheAccount,
                        ErrorCode = (int)ErrorCodes.ErrorLoggingOutOfTheAccount,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList()
                    };
                }

                else
                {
                    string userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == "uid")?.Value;
                    _tokenProvider.ClearToken();

                    var logoutUserDto = new LogoutUserDto
                    {
                        UserId = request.LogoutUser.UserId,
                    };

                    return new Result<LogoutUserDto>
                    {
                        Data = logoutUserDto,
                        SuccessMessage = "Вы успешно вышли из аккаунта",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<LogoutUserDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}