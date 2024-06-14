using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Products;
using PineappleSite.Presentation.Services.Products;

namespace PineappleSite.Presentation.Services;

public sealed class ProductService(
    ILocalStorageService localStorageService,
    IProductClient productClient,
    IMapper mapper,
    IHttpContextAccessor contextAccessor)
    : BaseProductService(localStorageService, productClient, contextAccessor), IProductService
{
    private readonly IProductClient _productClient = productClient;

    public async Task<ProductsCollectionResultViewModel<ProductViewModel>> GetAllProductsAsync()
    {
        AddBearerToken();
        try
        {
            var products = await _productClient.GetProductsAsync();

            if (products.IsSuccess)
            {
                return mapper.Map<ProductsCollectionResultViewModel<ProductViewModel>>(products);
            }

            return new ProductsCollectionResultViewModel<ProductViewModel>
            {
                StatusCode = products.StatusCode,
                ErrorMessage = products.ErrorMessage,
                ValidationErrors = string.Join(", ", products.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductsCollectionResultViewModel<ProductViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<ProductResultViewModel<ProductViewModel>> GetProductAsync(int productId)
    {
        AddBearerToken();
        try
        {
            var product = await _productClient.GetProductAsync(productId);

            if (product.IsSuccess)
            {
                return new ProductResultViewModel<ProductViewModel>
                {
                    StatusCode = product.StatusCode,
                    SuccessMessage = product.SuccessMessage,
                    Data = mapper.Map<ProductViewModel>(product.Data),
                };
            }

            return new ProductResultViewModel<ProductViewModel>
            {
                StatusCode = product.StatusCode,
                ErrorMessage = product.ErrorMessage,
                ValidationErrors = string.Join(", ", product.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductResultViewModel<ProductViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<ProductResultViewModel<int>> CreateProductAsync(CreateProductViewModel product)
    {
        AddBearerToken();
        try
        {
            FileParameter avatarFileParameter = null;
            if (product.Avatar is not null)
            {
                avatarFileParameter = new FileParameter(product.Avatar.OpenReadStream(), product.Avatar.FileName);
            }

            var apiResponse = await _productClient.CreateProductAsync(
                product.Name,
                product.Description,
                (ProductCategory?)product.ProductCategory,
                (double)product.Price,
                avatarFileParameter);

            if (apiResponse.IsSuccess)
            {
                return new ProductResultViewModel<int>
                {
                    Data = apiResponse.Data,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ProductResultViewModel<int>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductResultViewModel<int>()
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<ProductResultViewModel> UpdateProductAsync(int productId, UpdateProductViewModel product)
    {
        AddBearerToken();
        try
        {
            FileParameter avatarFileParameter = null;

            if (product.Avatar is not null)
            {
                avatarFileParameter = new FileParameter(product.Avatar.OpenReadStream(), product.Avatar.FileName);
            }

            var apiResponse = await _productClient.UpdateProductAsync(
                productId,
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
                return new ProductResultViewModel
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ProductResultViewModel
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductResultViewModel()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<ProductResultViewModel> DeleteProductAsync(int productId, DeleteProductViewModel product)
    {
        AddBearerToken();
        try
        {
            var deleteProductDto = mapper.Map<DeleteProductDto>(product);
            var apiResponse = await _productClient.DeleteProductAsync(deleteProductDto.Id, deleteProductDto);

            if (apiResponse.IsSuccess)
            {
                return new ProductResultViewModel
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ProductResultViewModel<ProductViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductResultViewModel<ProductViewModel>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<ProductsCollectionResultViewModel<bool>> DeleteProductsAsync(
        DeleteProductsViewModel product)
    {
        AddBearerToken();
        try
        {
            var deleteProductsDto = mapper.Map<DeleteProductsDto>(product);
            var apiResponse = await _productClient.DeleteProductsAsync(deleteProductsDto);

            if (apiResponse.IsSuccess)
            {
                return new ProductsCollectionResultViewModel<bool>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ProductsCollectionResultViewModel<bool>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductsCollectionResultViewModel<bool>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }
}