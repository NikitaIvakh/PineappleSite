using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Identities;
using PineappleSite.Presentation.Services.Identities;

namespace PineappleSite.Presentation.Services
{
    public class IdentityService(ILocalStorageService localStorageService, IIdentityClient identityClient, IMapper mapper) : BaseIdentityService(localStorageService, identityClient), IIdentityService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IIdentityClient _identityClient = identityClient;
        private readonly IMapper _mapper = mapper;

        public async Task<IdentityResponseViewModel> LoginAsync(AuthRequestViewModel authRequestViewModel)
        {
            IdentityResponseViewModel response = new();
            AuthRequest authRequest = _mapper.Map<AuthRequest>(authRequestViewModel);
            BaseIdentityResponse apiResponse = await _identityClient.LoginAsync(authRequest);

            if (apiResponse.IsSuccess)
            {
                response.IsSuccess = true;
                response.Data = apiResponse.Data;
                response.Message = apiResponse.Message;
            }

            else
            {
                foreach (string error in apiResponse.ValidationErrors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }
            }

            return response;
        }

        public async Task<IdentityResponseViewModel> RegisterAsync(RegisterRequestViewModel registerRequestViewModel)
        {
            IdentityResponseViewModel response = new();
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(registerRequestViewModel);
            BaseIdentityResponse apiResponse = await _identityClient.RegisterAsync(registerRequest);

            if (apiResponse.IsSuccess)
            {
                response.IsSuccess = true;
                response.Data = apiResponse.Data;
                response.Message = apiResponse.Message;
            }

            else
            {
                foreach (string error in apiResponse.ValidationErrors)
                {
                    response.ValidationErrors += error + Environment.NewLine;
                }
            }

            return response;
        }
    }
}