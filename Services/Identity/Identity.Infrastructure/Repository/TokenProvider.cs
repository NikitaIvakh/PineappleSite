using Identity.Application.Utilities;
using Identity.Domain.Interface;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Services
{
    public class TokenProvider(IHttpContextAccessor httpContextAccessor) : ITokenProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response?.Cookies.Append(StaticDetails.TokenCookie, token);
        }

        public string? GetToken()
        {
            string? token = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);

            return hasToken is true ? token : null;
        }


        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response?.Cookies.Delete(StaticDetails.TokenCookie);
        }
    }
}