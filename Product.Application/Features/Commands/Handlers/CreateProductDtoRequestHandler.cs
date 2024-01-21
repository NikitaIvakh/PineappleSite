using AutoMapper;
using MediatR;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;
using Product.Core.Entities.Producrs;

namespace Product.Application.Features.Commands.Handlers
{
    public class CreateProductDtoRequestHandler(IProductDbContext context, IMapper mapper, ICreateProductDtoValidator createValidator) : IRequestHandler<CreateProductDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ICreateProductDtoValidator _createValidator = createValidator;
        private readonly ProductAPIResponse _productAPIResponse = new ProductAPIResponse();

        public async Task<ProductAPIResponse> Handle(CreateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _createValidator.ValidateAsync(request.CreateProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    _productAPIResponse.IsSuccess = false;
                    _productAPIResponse.Message = "Ошибка создания продукта";
                    _productAPIResponse.ValidationErrors = validator.Errors.Select(e => e.ErrorMessage).ToList();
                }

                else
                {
                    var product = _mapper.Map<ProductEntity>(request.CreateProduct);

                    await _context.Products.AddAsync(product, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);

                    _productAPIResponse.IsSuccess = true;
                    _productAPIResponse.Message = "Продукт успешно добавлен";
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