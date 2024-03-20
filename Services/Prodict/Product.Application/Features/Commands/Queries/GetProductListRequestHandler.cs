using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Features.Requests.Queries;
using Product.Application.Resources;
using Product.Domain.DTOs;
using Product.Domain.Entities.Producrs;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;
using Serilog;

namespace Product.Application.Features.Commands.Queries
{
    public class GetProductListRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetProductListRequest, CollectionResult<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetProductListRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheProductKey";

        public async Task<CollectionResult<ProductDto>> Handle(GetProductListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out IReadOnlyCollection<ProductDto>? products))
                {
                    return new CollectionResult<ProductDto>
                    {
                        Data = products,
                        Count = products.Count,
                        SuccessCode = (int)SuccessCode.Ok,
                    };
                }

                else
                {
                    products = await _repository.GetAll().Select(key => new ProductDto
                    {
                        Id = key.Id,
                        Name = key.Name,
                        Description = key.Description,
                        ProductCategory = key.ProductCategory,
                        Price = key.Price,
                        ImageUrl = key.ImageUrl,
                        ImageLocalPath = key.ImageLocalPath,
                    }).OrderBy(key => key.Id).ToListAsync(cancellationToken);

                    if (products is null || products.Count == 0)
                    {
                        _logger.Warning(ErrorMessage.ProductsNotFound, ErrorCodes.ProductsNotFound);
                        return new CollectionResult<ProductDto>
                        {
                            Data = [],
                            SuccessCode = (int)SuccessCode.Ok,
                            SuccessMessage = SuccessMessage.ProductsNotFound,
                        };
                    }

                    else
                    {
                        _memoryCache.Remove(products);
                        _memoryCache.Set(cacheKey, products);

                        return new CollectionResult<ProductDto>
                        {
                            Data = products,
                            Count = products.Count,
                            SuccessCode = (int)SuccessCode.Ok,
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new CollectionResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.ProductPriceNotValid]
                };
            }
        }
    }
}