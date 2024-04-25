using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Features.Requests.Queries;
using Product.Application.Resources;
using Product.Domain.DTOs;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Commands.Queries;

public class GetProductsRequestHandler(IProductRepository repository, IMemoryCache memoryCache)
    : IRequestHandler<GetProductsRequest, CollectionResult<GetProductsDto>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<CollectionResult<GetProductsDto>> Handle(GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<GetProductsDto>? products))
            {
                return new CollectionResult<GetProductsDto>
                {
                    Data = products,
                    Count = products!.Count,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyGot", SuccessMessage.Culture)
                };
            }

            var getAllProducts = await repository.GetAll().ToListAsync(cancellationToken);

            if (getAllProducts.Count == 0)
            {
                return new CollectionResult<GetProductsDto>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage = SuccessMessage.ResourceManager.GetString("ProductsNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        SuccessMessage.ResourceManager.GetString("ProductsNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ],
                };
            }

            var getProducts = getAllProducts.Select(key => new GetProductsDto
            (
                Id: key.Id,
                Name: key.Name,
                Description: key.Description,
                ProductCategory: key.ProductCategory,
                Price: key.Price,
                ImageUrl: key.ImageUrl,
                ImageLocalPath: key.ImageLocalPath
            )).OrderByDescending(key => key.Id).ToList();

            memoryCache.Remove(CacheKey);

            return new CollectionResult<GetProductsDto>
            {
                Data = getProducts,
                Count = getProducts.Count,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyGot", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new CollectionResult<GetProductsDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}