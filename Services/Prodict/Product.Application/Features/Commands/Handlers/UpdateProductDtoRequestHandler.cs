﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
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
    public class UpdateProductDtoRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger, IMapper mapper, IUpdateProductDtoValidator updateValidator, IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache) : IRequestHandler<UpdateProductDtoRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<UpdateProductDtoRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IUpdateProductDtoValidator _updateValidator = updateValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheProductKey";

        public async Task<Result<ProductDto>> Handle(UpdateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        {"Name", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"Description", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"ProductCategory", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                        {"Price", validator.Errors.Select(x => x.ErrorMessage).ToList() },
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<ProductDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.ProductNotUpdated,
                                ErrorCode = (int)ErrorCodes.ProductNotUpdated,
                            };
                        }
                    }

                    return new Result<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductNotUpdated,
                        ErrorCode = (int)ErrorCodes.ProductNotUpdated,
                        ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var product = await _repository.GetAll().FirstOrDefaultAsync(key => key.Id == request.UpdateProduct.Id, cancellationToken);

                    if (product is null)
                    {
                        return new Result<ProductDto>
                        {
                            ErrorMessage = ErrorMessage.ProductNotUpdatedNull,
                            ErrorCode = (int)ErrorCodes.ProductNotUpdatedNull,
                            ValidationErrors = [ErrorMessage.ProductNotUpdatedNull]
                        };
                    }

                    else
                    {
                        product.Name = request.UpdateProduct.Name.Trim();
                        product.Description = request.UpdateProduct.Description.Trim();
                        product.ProductCategory = request.UpdateProduct.ProductCategory;
                        product.Price = request.UpdateProduct.Price;

                        await _repository.UpdateAsync(product);

                        if (request.UpdateProduct.Avatar is not null)
                        {
                            if (!string.IsNullOrEmpty(product.ImageLocalPath))
                            {
                                var fileNameToDelete = $"Id_{product.Id}*";
                                var filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                                var files = Directory.GetFiles(filePathToDelete, fileNameToDelete + ".*");

                                foreach (var file in files)
                                {
                                    File.Delete(file);
                                }

                                product.ImageUrl = null;
                                product.ImageLocalPath = null;

                                await _repository.UpdateAsync(product);
                            }

                            string fileName = $"Id_{product.Id}------{Guid.NewGuid()}"+ Path.GetExtension(request.UpdateProduct.Avatar.FileName);
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                            string fileDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                            if (!Directory.Exists(filePath))
                            {
                                Directory.CreateDirectory(filePath);
                            }

                            var fileFullPath = Path.Combine(fileDirectory, fileName);

                            using (FileStream fileStream = new(fileFullPath, FileMode.Create))
                            {
                                request.UpdateProduct.Avatar.CopyTo(fileStream);
                            };

                            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
                            product.ImageUrl = Path.Combine(baseUrl, "ProductImages", fileName);
                            product.ImageLocalPath = filePath;

                            await _repository.UpdateAsync(product);

                            _memoryCache.Remove(product);
                            _memoryCache.Set(cacheKey, product);

                            return new Result<ProductDto>
                            {
                                SuccessCode = (int)SuccessCode.Updated,
                                Data = _mapper.Map<ProductDto>(product),
                                SuccessMessage = SuccessMessage.ProductsSuccessfullyUpdated,
                            };
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(product.ImageLocalPath))
                            {
                                var fileNameToDelete = product.Id;
                                var filePathToDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");

                                var files = Directory.GetFiles(filePathToDelete, fileNameToDelete + ".*");

                                foreach (var file in files)
                                {
                                    File.Delete(file);
                                }
                            }

                            request.UpdateProduct.ImageUrl = product.ImageUrl;
                            request.UpdateProduct.ImageLocalPath = product.ImageLocalPath;

                            await _repository.UpdateAsync(product);

                            _memoryCache.Remove(product);
                            _memoryCache.Set(cacheKey, product);

                            return new Result<ProductDto>
                            {
                                SuccessCode = (int)SuccessCode.Updated,
                                Data = _mapper.Map<ProductDto>(product),
                                SuccessMessage = SuccessMessage.ProductsSuccessfullyUpdated,
                            };
                        }
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
                    ValidationErrors = [ErrorMessage.InternalServerError]
                };
            }
        }
    }
}