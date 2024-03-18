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
    public class GetProductDetailsRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger, IMemoryCache memoryCache) : IRequestHandler<GetProductDetailsRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetProductDetailsRequestHandler>();
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheProductKey";

        public async Task<Result<ProductDto>> Handle(GetProductDetailsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out ProductDto? productDto))
                {
                    return new Result<ProductDto>
                    {
                        Data = productDto,
                    };
                }

                productDto = await _repository.GetAll().Select(key => new ProductDto
                {
                    Id = key.Id,
                    Name = key.Name,
                    Description = key.Description,
                    ProductCategory = key.ProductCategory,
                    Price = key.Price,
                    ImageUrl = key.ImageUrl,
                    ImageLocalPath = key.ImageLocalPath,
                }).FirstOrDefaultAsync(key => key.Id == request.Id, cancellationToken);

                if (productDto is null)
                {
                    _logger.Warning($"Прродукт с {request.Id} не найден");
                    return new Result<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductNotFound,
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                    };
                }

                else
                {
                    _memoryCache.Set(cacheKey, productDto);

                    return new Result<ProductDto>
                    {
                        Data = productDto,
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                _memoryCache.Remove(cacheKey);
                return new Result<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}