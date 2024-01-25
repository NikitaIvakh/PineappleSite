using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Product.Application.DTOs.Validator;
using Product.Application.Exceptions;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;

namespace Product.Application.Features.Commands.Handlers
{
    public class UpdateProductDtoRequestHandler(IProductDbContext context, IMapper mapper, IUpdateProductDtoValidator updateValidator, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateProductDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly IUpdateProductDtoValidator _updateValidator = updateValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ProductAPIResponse _productAPIResponse = new();

        public async Task<ProductAPIResponse> Handle(UpdateProductDtoRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await _updateValidator.ValidateAsync(request.UpdateProduct, cancellationToken);

                if (!validator.IsValid)
                {
                    _productAPIResponse.IsSuccess = false;
                    _productAPIResponse.Message = "Ошибка обновления продукта";
                    _productAPIResponse.ValidationErrors = validator.Errors.Select(x => x.ErrorMessage).ToList();
                }

                else
                {
                    var product = await _context.Products.FirstOrDefaultAsync(key => key.Id == request.UpdateProduct.Id, cancellationToken) ??
                        throw new NotFoundException($"У продукта ({request.UpdateProduct.Name}) не существует идкетификатора: ", request.UpdateProduct.Id);

                    _mapper.Map(request.UpdateProduct, product);
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync(cancellationToken);

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
                        product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                        product.ImageLocalPath = filePath;
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
                    }

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