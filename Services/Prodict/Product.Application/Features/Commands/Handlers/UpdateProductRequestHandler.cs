using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Resources;
using Product.Application.Validations;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Commands.Handlers;

public sealed class UpdateProductRequestHandler(
    IProductRepository repository,
    UpdateValidator updateValidator,
    IHttpContextAccessor httpContextAccessor,
    IMemoryCache memoryCache) : IRequestHandler<UpdateProductRequest, Result<Unit>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<Result<Unit>> Handle(UpdateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await updateValidator.ValidateAsync(request.UpdateProduct, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "Name", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Description", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "ProductCategory", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    { "Price", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                };

                foreach (var error in errorMessages)
                {
                    if (errorMessages.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("ProductNotUpdated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductNotUpdated", ErrorMessage.Culture),
                };
            }

            var product = await repository.GetAll()
                .FirstOrDefaultAsync(key => key.Id == request.UpdateProduct.Id, cancellationToken);

            if (product is null)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductNotUpdatedNull"),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ProductNotUpdatedNull", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            product.Name = request.UpdateProduct.Name.Trim();
            product.Description = request.UpdateProduct.Description.Trim();
            product.ProductCategory = request.UpdateProduct.ProductCategory;
            product.Price = request.UpdateProduct.Price;

            await repository.UpdateAsync(product);

            if (request.UpdateProduct.Avatar is not null)
            {
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var fileNameToDelete = $"Id_{product.Id}*";
                    var filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",
                        "ProductImages");

                    var files = Directory.GetFiles(filePathToDelete, fileNameToDelete + ".*");

                    foreach (var file in files)
                    {
                        File.Delete(file);
                    }

                    product.ImageUrl = null;
                    product.ImageLocalPath = null;

                    await repository.UpdateAsync(product);
                }

                var fileName = $"Id_{product.Id}------{Guid.NewGuid()}" +
                               Path.GetExtension(request.UpdateProduct.Avatar.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                var fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                var fileFullPath = Path.Combine(fileDirectory, fileName);

                await using (FileStream fileStream = new(fileFullPath, FileMode.Create))
                {
                    await request.UpdateProduct.Avatar.CopyToAsync(fileStream, cancellationToken);
                }

                var baseUrl =
                    $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}{httpContextAccessor.HttpContext.Request.PathBase.Value}";
                product.ImageUrl = Path.Combine(baseUrl, "ProductImages", fileName);
                product.ImageLocalPath = filePath;
            }

            else
            {
                request.UpdateProduct = request.UpdateProduct with { ImageUrl = product.ImageUrl };
                request.UpdateProduct = request.UpdateProduct with { ImageLocalPath = product.ImageLocalPath };
            }

            await repository.UpdateAsync(product);
            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Modify,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductsSuccessfullyUpdated", SuccessMessage.Culture),
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