using Order.Domain.DTOs;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;
using System.Text.Json;

namespace Order.Infrastructure.Repository.Services;

public sealed class UserService(IHttpClientFactory httpClientFactory) : IUserService
{
    public async Task<Result<GetUserDto>> GetUserAsync(string userId)
    {
        var httpClient = httpClientFactory.CreateClient("User");
        var response = await httpClient.GetAsync($"/api/users/GetUser/{userId}");

        if (!response.IsSuccessStatusCode)
        {
            return new Result<GetUserDto>();
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        return (await JsonSerializer.DeserializeAsync<Result<GetUserDto>>(stream, jsonSerializerOptions))!;
    }
}