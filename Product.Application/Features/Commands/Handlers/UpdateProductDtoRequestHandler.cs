using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Validator;
using Product.Application.Exceptions;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;

namespace Product.Application.Features.Commands.Handlers
{
    public class UpdateProductDtoRequestHandler(IProductDbContext context, IMapper mapper, IUpdateProductDtoValidator updateValidator) : IRequestHandler<UpdateProductDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IUpdateProductDtoValidator _updateValidator = updateValidator;
        private readonly ProductAPIResponse _productAPIResponse = new ProductAPIResponse();

        public async Task<ProductAPIResponse> Handle(UpdateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    _productAPIResponse.IsSuccess = false;
                    _productAPIResponse.Message = "Ошибка обновления";
                    _productAPIResponse.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var product = await _context.Products.FirstOrDefaultAsync(key => key.Id == request.UpdateProduct.Id, cancellationToken) ??
                        throw new NotFoundException($"У продукта {request.UpdateProduct.Name} не существует идкетификатора: ", request.UpdateProduct.Id);

                    _mapper.Map(request.UpdateProduct, product);
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync(cancellationToken);

                    _productAPIResponse.IsSuccess = true;
                    _productAPIResponse.Message = "Продукт успешно обновлен";
                    _productAPIResponse.Id = product.Id;

                    return _productAPIResponse;
                }
            }

            catch (Exception exception)
            {
                _productAPIResponse.IsSuccess = false;
                _productAPIResponse.Message = exception.Message;
            }

            return _productAPIResponse;
        }
    }
}