using MediatR;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;

namespace Product.Application.Features.Commands.Handlers
{
    public class DeleteProductsDtoRequestHandler(IProductDbContext context, IDeleteProductsDtoValidator validations) : IRequestHandler<DeleteProductsDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IDeleteProductsDtoValidator _deleteProductsValid = validations;
        private readonly ProductAPIResponse _productAPIResponse = new();

        public async Task<ProductAPIResponse> Handle(DeleteProductsDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteProductsValid.ValidateAsync(request.DeleteProducts, cancellationToken);

                if (!validator.IsValid)
                {
                    _productAPIResponse.IsSuccess = false;
                    _productAPIResponse.Message = "Ошибка удаления продуктов";
                    _productAPIResponse.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var products = await _context.Products.Where(key => request.DeleteProducts.ProductIds.Contains(key.Id)).ToListAsync(cancellationToken);

                    _context.Products.RemoveRange(products);
                    await _context.SaveChangesAsync(cancellationToken);

                    _productAPIResponse.IsSuccess = true;
                    _productAPIResponse.Message = "Продукты успешн удалены";

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