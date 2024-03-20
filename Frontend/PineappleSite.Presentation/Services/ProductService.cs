using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Services
{
    public class ProductService(ILocalStorageService localStorageService, IProductClient productClient, IMapper mapper, IHttpContextAccessor contextAccessor)
        : BaseProductService(localStorageService, productClient, contextAccessor), IProductService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IProductClient _productClient = productClient;
        private readonly IMapper _mapper = mapper;
        private readonly IHttpContextAccessor _contextAccessor = contextAccessor;

        public async Task<ProductsCollectionResultViewModel<ProductViewModel>> GetAllProductsAsync()
        {
            try
            {
                var products = await _productClient.ProductGETAsync();

                if (products.IsSuccess)
                {
                    return _mapper.Map<ProductsCollectionResultViewModel<ProductViewModel>>(products);
                }

                else
                {
                    foreach (var error in products.ValidationErrors)
                    {
                        return new ProductsCollectionResultViewModel<ProductViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = products.ErrorCode,
                            ErrorMessage = products.ErrorMessage,
                        };
                    }
                }

                return new ProductsCollectionResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                return new ProductsCollectionResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> GetProductAsync(int id)
        {
            try
            {
                var product = await _productClient.ProductGET2Async(id);

                if (product.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        Data = _mapper.Map<ProductViewModel>(product.Data),
                    };
                }

                else
                {
                    foreach (string error in product.ValidationErrors)
                    {
                        return new ProductResultViewModel<ProductViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = product.ErrorCode,
                            ErrorMessage = product.ErrorMessage,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> CreateProductAsync(CreateProductViewModel product)
        {
            AddBearerToken();
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
                        SuccessCode = apiREsponse.SuccessCode,
                        SuccessMessage = apiREsponse.SuccessMessage,
                        Data = _mapper.Map<ProductViewModel>(apiREsponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiREsponse.ValidationErrors)
                    {
                        return new ProductResultViewModel<ProductViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiREsponse.ErrorCode,
                            ErrorMessage = apiREsponse.ErrorMessage,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> UpdateProductAsync(int id, UpdateProductViewModel product)
        {
            AddBearerToken();
            try
            {
                FileParameter avatarFileParameter = null;

                if (product.Avatar is not null)
                {
                    avatarFileParameter = new FileParameter(product.Avatar.OpenReadStream(), product.Avatar.FileName);
                }

                ProductDtoResult apiResponse = await _productClient.ProductPUTAsync(
                    id,
                    product.Id,
                    product.Name,
                    product.Description,
                    (ProductCategory?)product.ProductCategory,
                    product.Price,
                    avatarFileParameter,
                    product.ImageUrl,
                    product.ImageLocalPath);

                if (apiResponse.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ProductResultViewModel<ProductViewModel>> DeleteProductAsync(int id, DeleteProductViewModel product)
        {
            AddBearerToken();
            try
            {
                DeleteProductDto deleteProductDto = _mapper.Map<DeleteProductDto>(product);
                ProductDtoResult apiResponse = await _productClient.ProductDELETE2Async(deleteProductDto.Id, deleteProductDto);

                if (apiResponse.IsSuccess)
                {
                    return new ProductResultViewModel<ProductViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ProductResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ProductResultViewModel<ProductViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ProductsCollectionResultViewModel<ProductViewModel>> DeleteProductsAsync(DeleteProductsViewModel product)
        {
            AddBearerToken();
            try
            {
                DeleteProductsDto deleteProductsDto = _mapper.Map<DeleteProductsDto>(product);
                ProductDtoCollectionResult apiResponse = await _productClient.ProductDELETEAsync(deleteProductsDto);

                if (apiResponse.IsSuccess)
                {
                    return new ProductsCollectionResultViewModel<ProductViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
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
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ProductsCollectionResultViewModel<ProductViewModel>();
            }

            catch (ProductExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ProductsCollectionResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ProductsCollectionResultViewModel<ProductViewModel>()
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertProductException(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ProductsCollectionResultViewModel<ProductViewModel>()
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }
    }
}