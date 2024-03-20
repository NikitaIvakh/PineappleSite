using Microsoft.AspNetCore.Http;
using Order.Domain.DTOs;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;
using System.Text.Json;

namespace Order.Infrastructure.Repository.Services
{
    public class UserService(IHttpClientFactory httpClientFactory) : IUserService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<Result<UserWithRolesDto>> GetUserAsync(string userId)
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("User");
            var response = await httpClient.GetAsync($"/api/User/GetUserById/{userId}");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();

                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true,
                };

                var options = jsonSerializerOptions;
                return await JsonSerializer.DeserializeAsync<Result<UserWithRolesDto>>(stream, options);
            }

            return new Result<UserWithRolesDto>();
        }
    }
}