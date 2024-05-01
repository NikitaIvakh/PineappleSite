using Order.Domain.DTOs;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;
using System.Text.Json;

namespace Order.Infrastructure.Repository.Services;

public sealed class ProductService(IHttpClientFactory httpClientFactory) : IProductService
{
    public async Task<CollectionResult<ProductDto>> GetProductsAsync()
    {
        var httpClient = httpClientFactory.CreateClient("Product");
        var response = await httpClient.GetAsync($"/api/products/GetProducts");

        if (!response.IsSuccessStatusCode)
        {
            return new CollectionResult<ProductDto>();
        }

        await using var stream = await response.Content.ReadAsStreamAsync();
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        return (await JsonSerializer.DeserializeAsync<CollectionResult<ProductDto>>(stream, jsonSerializerOptions))!;
    }
}