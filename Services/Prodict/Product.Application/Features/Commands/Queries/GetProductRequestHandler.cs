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

public sealed class GetProductRequestHandler(IProductRepository repository, IMemoryCache memoryCache, IMapper mapper)
    : IRequestHandler<GetProductRequest, Result<ProductDto>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<Result<ProductDto>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out ProductDto? productDto))
            {
                return new Result<ProductDto>
                {
                    Data = productDto,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ProductSuccessfullyGot", SuccessMessage.Culture)
                };
            }

            var product =
                await repository.GetAll().FirstOrDefaultAsync(key => key.Id == request.Id, cancellationToken);

            if (product is null)
            {
                return new Result<ProductDto>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ProductNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            memoryCache.Remove(CacheKey);

            return new Result<ProductDto>
            {
                Data = mapper.Map<ProductDto>(product),
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyGot", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<ProductDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}