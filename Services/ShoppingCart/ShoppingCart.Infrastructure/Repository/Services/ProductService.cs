using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Domain.Results;
using System.Text.Json;

namespace ShoppingCart.Infrastructure.Repository.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<CollectionResult<ProductDto>> GetProductListAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Product");
            var response = await httpClient.GetAsync($"/api/Product");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true,
                };

                var options = jsonSerializerOptions;
                return await JsonSerializer.DeserializeAsync<CollectionResult<ProductDto>>(stream, options);
            }

            return new CollectionResult<ProductDto>();
        }
    }
}