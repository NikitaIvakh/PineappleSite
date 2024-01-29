﻿using AutoMapper;
using MediatR;
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
    public class DeleteProductDtoRequestHandler(IBaseRepository<ProductEntity> repository, IDeleteProductDtoValidator deleteValidator, ILogger logger, IMapper mapper) : IRequestHandler<DeleteProductDtoRequest, Result<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly IDeleteProductDtoValidator _deleteValidator = deleteValidator;
        private readonly ILogger _logger = logger.ForContext<DeleteProductDtoRequestHandler>();
        private readonly IMapper _mapper = mapper;

        public async Task<Result<ProductDto>> Handle(DeleteProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    _logger.Warning($"Ошибка валидации");
                    return new Result<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductNotDeleted,
                        ErrorCode = (int)ErrorCodes.ProductNotDeleted,
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
                        };
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(product.ImageLocalPath))
                        {
                            string fileName = product.Id + ".jpg";
                            string filePath = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath, fileName);

                            if (File.Exists(filePath))
                            {
                                File.Delete(filePath);
                            }
                        }

                        else
                        {
                            await _repository.DeleteAsync(product);
                        }
                    }

                    return new Result<ProductDto>
                    {
                        Data = _mapper.Map<ProductDto>(product),
                        SuccessMessage = "Продукт успешно удален",
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
                };
            }
        }
    }
}