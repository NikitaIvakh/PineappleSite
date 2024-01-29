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
    public class UpdateProductDtoRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger, IMapper mapper, IUpdateProductDtoValidator updateValidator, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateProductDtoRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<UpdateProductDtoRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IUpdateProductDtoValidator _updateValidator = updateValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<ProductDto>> Handle(UpdateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateProduct, cancellationToken);

                if (!validator.IsValid)
                {
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
                        };
                    }

                    else
                    {
                        product.Name = request.UpdateProduct.Name;
                        product.Description = request.UpdateProduct.Description;
                        product.ProductCategory = request.UpdateProduct.ProductCategory;
                        product.Price = request.UpdateProduct.Price;

                        await _repository.UpdateAsync(product);

                        if (request.UpdateProduct.Avatar is not null)
                        {
                            if (!string.IsNullOrEmpty(product.ImageLocalPath) || !string.IsNullOrEmpty(product.ImageUrl))
                            {
                                var oldFilePasthDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                                FileInfo fileInfo = new(oldFilePasthDirectory);

                                if (fileInfo.Exists)
                                    fileInfo.Delete();
                            }

                            Random random = new();
                            int randomNumber = random.Next(1, 120001);

                            string fileName = $"{product.Id}{randomNumber}" + Path.GetExtension(request.UpdateProduct.Avatar.FileName);
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
                        }

                        else
                        {
                            if (!string.IsNullOrEmpty(product.ImageLocalPath))
                            {
                                var oldFilePasthDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                                FileInfo fileInfo = new(oldFilePasthDirectory);

                                if (fileInfo.Exists)
                                    fileInfo.Delete();
                            }

                            product.ImageUrl = null;
                            product.ImageLocalPath = null;

                            await _repository.UpdateAsync(product);
                        }

                        return new Result<ProductDto>
                        {
                            SuccessMessage = "Продукт успешно обновлен",
                            Data = _mapper.Map<ProductDto>(product),
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