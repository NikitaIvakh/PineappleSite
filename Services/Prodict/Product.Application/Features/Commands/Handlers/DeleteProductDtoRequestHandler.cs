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
    public class DeleteProductDtoRequestHandler(IBaseRepository<ProductEntity> repository, IDeleteProductDtoValidator deleteValidator, ILogger logger, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<DeleteProductDtoRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly IDeleteProductDtoValidator _deleteValidator = deleteValidator;
        private readonly ILogger _logger = logger.ForContext<DeleteProductDtoRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheProductKey";

        public async Task<Result<ProductDto>> Handle(DeleteProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    var existErrorMessages = new Dictionary<string, List<string>>
                    {
                        {"Id", validator.Errors.Select(key => key.ErrorMessage).ToList() },
                    };

                    foreach (var error in existErrorMessages)
                    {
                        if (existErrorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<ProductDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.ProductNotDeleted,
                                ErrorCode = (int)ErrorCodes.ProductNotDeleted,
                            };
                        }
                    }

                    _logger.Warning($"Ошибка валидации");
                    return new Result<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductNotDeleted,
                        ErrorCode = (int)ErrorCodes.ProductNotDeleted,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList()
                    };
                }

                else
                {
                    var product = await _repository.GetAll().FirstOrDefaultAsync(key => key.Id == request.DeleteProduct.Id, cancellationToken);

                    if (product is null)
                    {
                        return new Result<ProductDto>
                        {
                            ErrorMessage = ErrorMessage.ProductNotFound,
                            ErrorCode = (int)ErrorCodes.ProductNotFound,
                            ValidationErrors = [ErrorMessage.ProductNotFound]
                        };
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(product.ImageLocalPath))
                        {
                            string fileName = $"Id_{product.Id}*";
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                            var files = Directory.GetFiles(filePath, fileName + ".*");

                            foreach (var file in files)
                            {
                                File.Delete(file);
                            }
                        }

                        await _repository.DeleteAsync(product);
                    }

                    var products = await _repository.GetAll().ToListAsync(cancellationToken);

                    _memoryCache.Remove(product);
                    _memoryCache.Remove(products);
                    _memoryCache.Set(cacheKey, product);
                    _memoryCache.Set(cacheKey, products);

                    return new Result<ProductDto>
                    {
                        SuccessCode = (int)SuccessCode.Deleted,
                        Data = _mapper.Map<ProductDto>(product),
                        SuccessMessage = SuccessMessage.ProductSuccessfullyDeleted,
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}