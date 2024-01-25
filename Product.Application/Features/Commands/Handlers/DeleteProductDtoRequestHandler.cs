using MediatR;
using Product.Application.DTOs.Validator;
using Product.Application.Exceptions;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;

namespace Product.Application.Features.Commands.Handlers
{
    public class DeleteProductDtoRequestHandler(IProductDbContext context, IDeleteProductDtoValidator deleteValidator) : IRequestHandler<DeleteProductDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IDeleteProductDtoValidator _deleteValidator = deleteValidator;
        private readonly ProductAPIResponse _productAPIResponse = new ProductAPIResponse();

        public async Task<ProductAPIResponse> Handle(DeleteProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _deleteValidator.ValidateAsync(request.DeleteProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    _productAPIResponse.IsSuccess = false;
                    _productAPIResponse.Message = "Ошибка удаления продукта";
                    _productAPIResponse.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var product = await _context.Products.FindAsync(new object[] { request.DeleteProduct.Id }, cancellationToken)
                        ?? throw new NotFoundException($"Продукта c идентификатором:", request.DeleteProduct.Id);

                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        string fileName = product.Id + ".jpg";
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath, fileName);

                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }

                    _context.Products.Remove(product);
                    await _context.SaveChangesAsync(cancellationToken);

                    _productAPIResponse.IsSuccess = true;
                    _productAPIResponse.Message = "Продукт успешно удален";
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