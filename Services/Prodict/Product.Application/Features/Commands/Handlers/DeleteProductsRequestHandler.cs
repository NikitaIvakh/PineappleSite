using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Resources;
using Product.Application.Validations;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Commands.Handlers;

public sealed class DeleteProductsRequestHandler(
    IProductRepository repository,
    DeleteProductsValidator validations,
    IMemoryCache memoryCache) : IRequestHandler<DeleteProductsRequest, CollectionResult<Unit>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<CollectionResult<Unit>> Handle(DeleteProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validator = await validations.ValidateAsync(request.DeleteProducts, cancellationToken);

            if (!validator.IsValid)
            {
                var existsErrorMessages = new Dictionary<string, List<string>>
                {
                    { "ProductIds", validator.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existsErrorMessages)
                {
                    if (existsErrorMessages.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new CollectionResult<Unit>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("ProductsNotDeleted", ErrorMessage.Culture),
                        };
                    }
                }

                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductsNotDeleted", ErrorMessage.Culture),
                };
            }

            var products = await repository.GetAll()
                .Where(key => request.DeleteProducts.ProductIds.Contains(key.Id))
                .ToListAsync(cancellationToken);

            if (products.Count == 0)
            {
                return new CollectionResult<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductsNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ProductsNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var fileName = $"Id_{product.Id}*";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                        "ProductImages");

                    var files = Directory.GetFiles(filePath, fileName + ".*");

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }
                }

                await repository.DeleteAsync(product);
            }

            memoryCache.Remove(CacheKey);

            return new CollectionResult<Unit>
            {
                Data = [Unit.Value],
                Count = products.Count,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}