using Order.Domain.DTOs;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;
using System.Text.Json;

namespace Order.Infrastructure.Repository.Services
{
    public class ProductService(IHttpClientFactory httpClientFactory) : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;

        public async Task<CollectionResult<ProductDto>> GetProductListAsync()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                UseDefaultCredentials = true
            };

            HttpClient httpClient = new HttpClient(handler);

            httpClient = _httpClientFactory.CreateClient("Product");
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