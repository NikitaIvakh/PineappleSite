using AutoMapper;
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

public class GetProductsRequestHandler(IProductRepository repository, IMemoryCache memoryCache, IMapper mapper)
    : IRequestHandler<GetProductsRequest, CollectionResult<ProductDto>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<CollectionResult<ProductDto>> Handle(GetProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out IReadOnlyCollection<ProductDto>? products))
            {
                return new CollectionResult<ProductDto>
                {
                    Data = products,
                    Count = products!.Count,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyGot", SuccessMessage.Culture)
                };
            }

            var getAllProducts = await repository
                    .GetAll()
                    .OrderBy(key => key.ProductCategory)
                    .ThenBy(key => key.Name)
                    .ToListAsync(cancellationToken);

            if (getAllProducts.Count == 0)
            {
                return new CollectionResult<ProductDto>
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

            memoryCache.Remove(CacheKey);

            return new CollectionResult<ProductDto>
            {
                Count = getAllProducts.Count,
                StatusCode = (int)StatusCode.Ok,
                Data = mapper.Map<IReadOnlyCollection<ProductDto>>(getAllProducts),
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyGot", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new CollectionResult<ProductDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}