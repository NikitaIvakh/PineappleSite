using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.ResultCart;
using ShoppingCart.Domain.Interfaces;
using System.Text.Json;

namespace ShoppingCart.Infrastructure.Repository
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<CollectionResult<ProductDto>> GetProductsAsync()
        {
            HttpClient httpClient = _httpClientFactory.CreateClient("Product");
            var response = await httpClient.GetAsync($"/api/Product");

            if (response.IsSuccessStatusCode)
            {
                using var stream = await response.Content.ReadAsStreamAsync();
                JsonSerializerOptions jsonSerializerOptions = new()
                {
                    PropertyNameCaseInsensitive = true
                };

                var options = jsonSerializerOptions;
                return await JsonSerializer.DeserializeAsync<CollectionResult<ProductDto>>(stream, options);
            }

            return new CollectionResult<ProductDto>();
        }
    }
}