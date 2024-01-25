using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Requests.Handlers;
using Product.Application.Interfaces;
using Product.Application.Response;
using Product.Core.Entities.Producrs;

namespace Product.Application.Features.Commands.Handlers
{
    public class CreateProductDtoRequestHandler(IProductDbContext context, IMapper mapper, ICreateProductDtoValidator createValidator, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateProductDtoRequest, ProductAPIResponse>
    {
        private readonly IProductDbContext _context = context;
        private readonly IMapper _mapper = mapper;
        private readonly ICreateProductDtoValidator _createValidator = createValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ProductAPIResponse _productAPIResponse = new();

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

                    if (request.CreateProduct.Avatar is not null)
                    {
                        string fileName = product.Id + Path.GetExtension(request.CreateProduct.Avatar.FileName);
                        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "ProductImages");
                        var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                        FileInfo fileInfo = new(directoryLocation);

                        if (fileInfo.Exists)
                            fileInfo.Delete();

                        var fileDirectory = Path.Combine(filePath, fileName);
                        using (FileStream fileStream = new(fileDirectory, FileMode.Create))
                        {
                            request.CreateProduct.Avatar.CopyTo(fileStream);
                        }

                        var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}{_httpContextAccessor.HttpContext.Request.PathBase.Value}";
                        product.ImageUrl = Path.Combine(baseUrl, "ProductImages", fileName);
                        product.ImageLocalPath = filePath;
                    }

                    else
                    {
                        product.ImageUrl = "https://placehold.co/600x400";
                    }

                    _context.Products.Update(product);
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