using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Response;
using Identity.Application.Services.IServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Identity.Application.Features.Identities.Commands.Commands
{
    public class LogoutUserRequestHandler(ITokenProvider tokenProvider, IActionContextAccessor actionContextAccessor, IUrlHelperFactory urlHelperFactory) : IRequestHandler<LogoutUserRequest, BaseIdentityResponse<Unit>>
    {
        private readonly ITokenProvider _tokenProvider = tokenProvider;
        private readonly IUrlHelperFactory _urlHelperFactory = urlHelperFactory;
        private readonly IActionContextAccessor _actionContextAccessor = actionContextAccessor;

        public async Task<BaseIdentityResponse<Unit>> Handle(LogoutUserRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseIdentityResponse<Unit>();

            try
            {
                var urlHelper = _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext);
                request.ReturnUrl ??= urlHelper.Content("/");
                _tokenProvider.ClearToken();

                response.IsSuccess = true;
                response.Message = "Успешный выход из аккаунта";
                response.Data = Unit.Value;

                return response;
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