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

public sealed class GetProductRequestHandler(IProductRepository repository, IMemoryCache memoryCache)
    : IRequestHandler<GetProductRequest, Result<GetProductDto>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<Result<GetProductDto>> Handle(GetProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (memoryCache.TryGetValue(CacheKey, out GetProductDto? productDto))
            {
                return new Result<GetProductDto>
                {
                    Data = productDto,
                    StatusCode = (int)StatusCode.Ok,
                    SuccessMessage =
                        SuccessMessage.ResourceManager.GetString("ProductSuccessfullyGot", SuccessMessage.Culture)
                };
            }

            var getProductFromDb =
                await repository.GetAll().FirstOrDefaultAsync(key => key.Id == request.Id, cancellationToken);

            if (getProductFromDb is null)
            {
                return new Result<GetProductDto>
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

            var getProduct = new GetProductDto
            (
                Id: getProductFromDb.Id,
                Name: getProductFromDb.Name,
                Description: getProductFromDb.Description,
                ProductCategory: getProductFromDb.ProductCategory,
                Price: getProductFromDb.Price,
                ImageUrl: getProductFromDb.ImageUrl,
                ImageLocalPath: getProductFromDb.ImageLocalPath
            );

            memoryCache.Remove(CacheKey);

            return new Result<GetProductDto>
            {
                Data = getProduct,
                StatusCode = (int)StatusCode.Ok,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyGot", SuccessMessage.Culture)
            };
        }

        catch (Exception ex)
        {
            memoryCache.Remove(CacheKey);
            return new Result<GetProductDto>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}