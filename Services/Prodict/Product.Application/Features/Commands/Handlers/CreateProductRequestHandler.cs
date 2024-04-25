using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Resources;
using Product.Application.Validations;
using Product.Domain.Entities.Producrs;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;

namespace Product.Application.Features.Commands.Handlers;

public sealed class CreateProductRequestHandler(
    IProductRepository repository,
    CreateValidator createValidator,
    IHttpContextAccessor httpContextAccessor,
    IMemoryCache memoryCache) : IRequestHandler<CreateProductRequest, Result<int>>
{
    private const string CacheKey = "cacheProductKey";

    public async Task<Result<int>> Handle(CreateProductRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validator = await createValidator.ValidateAsync(request.CreateProduct, cancellationToken);

            if (!validator.IsValid)
            {
                var errorMessages = new Dictionary<string, List<string>>
                {
                    { "Name", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "Description", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "ProductCategory", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                    { "Price", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                };

                foreach (var error in errorMessages)
                {
                    if (errorMessages.TryGetValue(error.Key, out var errorException))
                    {
                        return new Result<int>
                        {
                            ValidationErrors = errorException,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessage.ResourceManager.GetString("ProductNotCreated", ErrorMessage.Culture),
                        };
                    }
                }

                return new Result<int>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductNotCreated", ErrorMessage.Culture),
                };
            }

            var existProductName = await repository.GetAll()
                .FirstOrDefaultAsync(key => key.Name == request.CreateProduct.Name, cancellationToken);

            if (existProductName is not null)
            {
                return new Result<int>
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage = ErrorMessage.ResourceManager.GetString("ProductAlreadyExists", ErrorMessage.Culture),
                    ValidationErrors =
                    [
                        ErrorMessage.ResourceManager.GetString("ProductAlreadyExists", ErrorMessage.Culture) ??
                        string.Empty
                    ]
                };
            }

            var product = new ProductEntity
            {
                Name = request.CreateProduct.Name.Trim(),
                Description = request.CreateProduct.Description.Trim(),
                ProductCategory = request.CreateProduct.ProductCategory,
                Price = request.CreateProduct.Price,
            };

            await repository.CreateAsync(product);

            if (request.CreateProduct.Avatar is not null)
            {
                var fileName = $"Id_{product.Id}------{Guid.NewGuid()}" +
                               Path.GetExtension(request.CreateProduct.Avatar.FileName);

                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                FileInfo fileInfo = new(directoryLocation);

                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                var fileDirectory = Path.Combine(filePath, fileName);
                await using (FileStream fileStream = new(fileDirectory, FileMode.Create))
                {
                    await request.CreateProduct.Avatar.CopyToAsync(fileStream, cancellationToken);
                }

                var baseUrl =
                    $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host.Value}{httpContextAccessor.HttpContext.Request.PathBase.Value}";
                product.ImageUrl = Path.Combine(baseUrl, "ProductImages", fileName);
                product.ImageLocalPath = filePath;
            }

            else
            {
                product.ImageUrl = product.ImageUrl;
                product.ImageLocalPath = product.ImageLocalPath;
            }

            memoryCache.Remove(CacheKey);
            await repository.UpdateAsync(product);

            return new Result<int>
            {
                Data = product.Id,
                StatusCode = (int)StatusCode.Created,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyAdded", SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<int>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}