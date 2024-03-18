using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Resources;
using Product.Domain.DTOs;
using Product.Domain.Entities.Producrs;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;
using Serilog;

namespace Product.Application.Features.Commands.Handlers
{
    public class DeleteProductsDtoRequestHandler(IBaseRepository<ProductEntity> repository, IDeleteProductsDtoValidator validations, ILogger logger, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<DeleteProductsDtoRequest, CollectionResult<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly IDeleteProductsDtoValidator _deleteProductsValid = validations;
        private readonly ILogger _logger = logger.ForContext<DeleteProductsDtoRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheProductKey";

        public async Task<CollectionResult<ProductDto>> Handle(DeleteProductsDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteProductsValid.ValidateAsync(request.DeleteProducts, cancellationToken);

                if (!validator.IsValid)
                {
                    _logger.Warning($"Ошибка валидации");
                    return new CollectionResult<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductsNotDeleted,
                        ErrorCode = (int)ErrorCodes.ProductsNotDeleted,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var products = await _repository.GetAll().Where(key => request.DeleteProducts.ProductIds.Contains(key.Id)).ToListAsync(cancellationToken);

                    if (products.Count == 0 || products is null)
                    {
                        return new CollectionResult<ProductDto>
                        {
                            ErrorMessage = ErrorMessage.ProductsNotDeleted,
                            ErrorCode = (int)ErrorCodes.ProductsNotDeleted,
                        };
                    }

                    else
                    {
                        await _repository.DeleteListAsync(products);

                        foreach (var product in products)
                        {
                            if (!string.IsNullOrEmpty(product.ImageLocalPath))
                            {
                                var fileName = product.Id;
                                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                                var files = Directory.GetFiles(filePath, fileName + ".*");

                                foreach (var file in files)
                                {
                                    File.Delete(file);
                                }
                            }

                            _memoryCache.Remove(product);
                        }

                        var productsCache = await _repository.GetAll().ToListAsync(cancellationToken);

                        _memoryCache.Remove(productsCache);
                        _memoryCache.Set(cacheKey, productsCache);

                        return new CollectionResult<ProductDto>
                        {
                            Count = products.Count,
                            SuccessMessage = "Продукты успешно удалены",
                            Data = _mapper.Map<IReadOnlyCollection<ProductDto>>(products),
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new CollectionResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}