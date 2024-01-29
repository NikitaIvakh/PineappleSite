using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.Features.Requests.Queries;
using Product.Application.Resources;
using Product.Domain.DTOs;
using Product.Domain.Entities.Producrs;
using Product.Domain.Enum;
using Product.Domain.Interfaces;
using Product.Domain.ResultProduct;
using Serilog;

namespace Product.Application.Features.Commands.Queries
{
    public class GetProductListRequestHandler(IBaseRepository<ProductEntity> repository, ILogger logger) : IRequestHandler<GetProductListRequest, CollectionResult<ProductDto>>
    {
        private readonly IBaseRepository<ProductEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<GetProductListRequestHandler>();

        public async Task<CollectionResult<ProductDto>> Handle(GetProductListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                IReadOnlyCollection<ProductDto> products = await _repository.GetAll().Select(key => new ProductDto
                {
                    Id = key.Id,
                    Name = key.Name,
                    Description = key.Description,
                    ProductCategory = key.ProductCategory,
                    Price = key.Price,
                    ImageUrl = key.ImageUrl,
                    ImageLocalPath = key.ImageLocalPath,
                }).OrderBy(key => key.Id).ToListAsync(cancellationToken);

                if (products.Count == 0)
                {
                    _logger.Warning(ErrorMessage.ProductsNotFound, products.Count);
                    return new CollectionResult<ProductDto>
                    {
                        ErrorMessage = ErrorMessage.ProductsNotFound,
                        ErrorCode = (int)ErrorCodes.ProductsNotFound,
                    };
                }

                else
                {
                    return new CollectionResult<ProductDto>
                    {
                        Data = products,
                        Count = products.Count,
                    };
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