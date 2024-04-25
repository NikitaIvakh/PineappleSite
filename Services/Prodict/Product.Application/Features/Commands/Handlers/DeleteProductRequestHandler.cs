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

public sealed class DeleteProductRequestHandler(
    IProductRepository repository,
    DeleteValidator deleteValidator,
    IMemoryCache memoryCache) : IRequestHandler<DeleteProductRequest, Result<Unit>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<Result<Unit>> Handle(DeleteProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await deleteValidator.ValidateAsync(request.DeleteProduct, cancellationToken);

            if (!validator.IsValid)
            {
                var existErrorMessages = new Dictionary<string, List<string>>
                {
                    { "Id", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                };

                foreach (var error in existErrorMessages)
                {
                    if (existErrorMessages.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("ProductNotDeleted", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage =
                        ErrorMessage.ResourceManager.GetString("ProductNotDeleted", ErrorMessage.Culture),
                };
            }

            var product = await repository.GetAll()
                .FirstOrDefaultAsync(key => key.Id == request.DeleteProduct.Id, cancellationToken);

            if (product is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductNotFound", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ProductNotFound", ErrorMessage.Culture) ??
                        string.Empty
                    ],
                };
            }

            if (!string.IsNullOrEmpty(product.ImageLocalPath))
            {
                var fileName = $"Id_{product.Id}*";
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                var files = Directory.GetFiles(filePath, fileName + ".*");

                foreach (var file in files)
                {
                    File.Delete(file);
                }
            }

            await repository.DeleteAsync(product);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}