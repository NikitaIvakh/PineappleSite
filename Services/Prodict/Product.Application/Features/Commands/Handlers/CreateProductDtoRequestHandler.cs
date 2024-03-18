using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
    public class CreateProductDtoRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger, IMapper mapper, ICreateProductDtoValidator createValidator, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateProductDtoRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<CreateProductDtoRequestHandler>();
        private readonly ICreateProductDtoValidator _createValidator = createValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<ProductDto>> Handle(CreateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _createValidator.ValidateAsync(request.CreateProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        {"Name", validator.Errors.Select(key => key.ErrorMessage).ToList()},
                        {"Description", validator.Errors.Select(key => key.ErrorMessage).ToList()},
                        {"ProductCategory", validator.Errors.Select(key => key.ErrorMessage).ToList()},
                        {"Price", validator.Errors.Select(key => key.ErrorMessage).ToList()},
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var errorException))
                        {
                            return new Result<ProductDto>
                            {
                                ErrorMessage = ErrorMessage.ProductNotCreated,
                                ErrorCode = (int)ErrorCodes.ProductNotCreated,
                                ValidationErrors = errorException,
                            };
                        }
                    }

                    _logger.Warning("Ошибка валидации");
                    return new Result<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductNotCreated,
                        ErrorCode = (int)ErrorCodes.ProductNotCreated,
                        ValidationErrors = validator.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var product = await _repository.GetAll().FirstOrDefaultAsync(key => key.Name == request.CreateProduct.Name, cancellationToken);

                    if (product is not null)
                    {
                        return new Result<ProductDto>
                        {
                            ErrorMessage = ErrorMessage.ProductAlreadyExists,
                            ErrorCode = (int)ErrorCodes.ProductAlreadyExists,
                        };
                    }

                    else
                    {
                        product = new ProductEntity
                        {
                            Name = request.CreateProduct.Name.Trim(),
                            Description = request.CreateProduct.Description.Trim(),
                            ProductCategory = request.CreateProduct.ProductCategory,
                            Price = request.CreateProduct.Price,
                        };

                        await _repository.CreateAsync(product);

                        if (request.CreateProduct.Avatar is not null)
                        {
                            string fileName = product.Id + Path.GetExtension(request.CreateProduct.Avatar.FileName);
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                            var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                            FileInfo fileInfo = new(directoryLocation);

                            if (fileInfo.Exists)
                            {
                                fileInfo.Delete();
                            }

                            var fileDirectory = Path.Combine(filePath, fileName);
                            using (FileStream fileStream = new(fileDirectory, FileMode.Create))
                            {
                                request.CreateProduct.Avatar.CopyTo(fileStream);
                            }

                            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
                            product.ImageUrl = Path.Combine(baseUrl, "ProductImages", fileName);
                            product.ImageLocalPath = filePath;

                            await _repository.UpdateAsync(product);

                            return new Result<ProductDto>
                            {
                                SuccessMessage = "Продукт успешно добавлен",
                                Data = _mapper.Map<ProductDto>(product),
                            };
                        }

                        else
                        {
                            product.ImageUrl = "https://placehold.co/600x400";
                            await _repository.UpdateAsync(product);

                            return new Result<ProductDto>
                            {
                                SuccessMessage = "Продукт успешно добавлен",
                                Data = _mapper.Map<ProductDto>(product),
                            };
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);

                return new Result<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}