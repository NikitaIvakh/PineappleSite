using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Services
{
    public class ProductService(ILocalStorageService localStorageService, IProductClient productClient, IMapper mapper) : BaseProductService(localStorageService, productClient), IProductService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IProductClient _productClient = productClient;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductsCollectionResultViewModel<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productClient.ProductGETAsync();
            return _mapper.Map<ProductsCollectionResultViewModel<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductAsync(int id)
        {
            var product = await _productClient.ProductGET2Async(id);
            return _mapper.Map<ProductViewModel>(product.Data);
        }

        public async Task<ProductResultViewModel<ProductViewModel>> CreateProductAsync(CreateProductViewModel product)
        {
            try
            {
                FileParameter avatarFileParameter = null;
                if (product.Avatar is not null)
                {
                    avatarFileParameter = new FileParameter(product.Avatar.OpenReadStream(), product.Avatar.FileName);
                }

                ProductDtoResult apiREsponse = await _productClient.ProductPOSTAsync(
                    product.Name,
                    product.Description,
                    (ProductCategory?)product.ProductCategory,
                    product.Price,
                    avatarFileParameter);

                if (apiREsponse.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        Data = _mapper.Map<ProductViewModel>(apiREsponse.Data),
                        SuccessMessage = apiREsponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiREsponse.ValidationErrors)
                    {
                        return new ProductResultViewModel<ProductViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exception)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> UpdateProductAsync(int id, UpdateProductViewModel product)
        {
            try
            {
                FileParameter avatarFileParameter = null;
                if (product.Avatar is not null)
                {
                    avatarFileParameter = new FileParameter(product.Avatar.OpenReadStream(), product.Avatar.FileName);
                }

                ProductDtoResult apiResponse = await _productClient.ProductPUTAsync(
                    id.ToString(),
                    product.Id,
                    product.Name,
                    product.Description,
                    (ProductCategory?)product.ProductCategory,
                    product.Price,
                    avatarFileParameter);

                if (apiResponse.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<ProductViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ProductResultViewModel<ProductViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exception)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> DeleteProductAsync(int id, DeleteProductViewModel product)
        {
            try
            {
                DeleteProductDto deleteProductDto = _mapper.Map<DeleteProductDto>(product);
                ProductDtoResult apiResponse = await _productClient.ProductDELETE2Async(deleteProductDto.Id.ToString(), deleteProductDto);

                if (apiResponse.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<ProductViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ProductResultViewModel<ProductViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exception)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }

        public async Task<ProductsCollectionResultViewModel<ProductViewModel>> DeleteProductsAsync(DeleteProductsViewModel product)
        {
            try
            {
                DeleteProductsDto deleteProductsDto = _mapper.Map<DeleteProductsDto>(product);
                ProductDtoCollectionResult apiResponse = await _productClient.ProductDELETEAsync(deleteProductsDto);

                if (apiResponse.IsSuccess)
                {
                    return new ProductsCollectionResultViewModel<ProductViewModel>
                    {
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<IReadOnlyCollection<ProductViewModel>>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ProductsCollectionResultViewModel<ProductViewModel>
                        {
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new ProductsCollectionResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exception)
            {
                return new ProductsCollectionResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exception.Response,
                    ErrorCode = exception.StatusCode,
                };
            }
        }
    }
}