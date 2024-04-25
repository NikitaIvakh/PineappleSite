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

    public async Task<ProductsCollectionResultViewModel<GetProductsViewModel>> GetAllProductsAsync()
    {
        AddBearerToken();
        try
        {
            var products = await _productClient.GetProductsAsync();
            var getProducts = products.Data.Select(key => new GetProductsViewModel
            (
                Id: key.Id,
                Name: key.Name,
                Description: key.Description,
                ProductCategory: key.ProductCategory,
                Price: key.Price,
                ImageUrl: key.ImageUrl,
                ImageLocalPath: key.ImageLocalPath,
                Count: 1
            )).ToList();

            if (products.IsSuccess)
            {
                return new ProductsCollectionResultViewModel<GetProductsViewModel>()
                {
                    Data = getProducts,
                    Count = products.Count,
                    StatusCode = products.StatusCode,
                    SuccessMessage = products.SuccessMessage,
                };
            }

            return new ProductsCollectionResultViewModel<GetProductsViewModel>
            {
                StatusCode = products.StatusCode,
                ErrorMessage = products.ErrorMessage,
                ValidationErrors = string.Join(", ", products.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductsCollectionResultViewModel<GetProductsViewModel>
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<ProductResultViewModel<GetProductViewModel>> GetProductAsync(int id)
    {
        AddBearerToken();
        try
        {
            var product = await _productClient.GetProductAsync(id);
            var getProduct = new GetProductViewModel(
                Id: product.Data.Id,
                Name: product.Data.Name,
                Description: product.Data.Description,
                ProductCategory: product.Data.ProductCategory,
                Price: product.Data.Price,
                ImageUrl: product.Data.ImageUrl,
                ImageLocalPath: product.Data.ImageLocalPath,
                Count: 1);

            if (product.IsSuccess)
            {
                return new ProductResultViewModel<GetProductViewModel>
                {
                    Data = getProduct,
                    StatusCode = product.StatusCode,
                    SuccessMessage = product.SuccessMessage,
                };
            }

            return new ProductResultViewModel<GetProductViewModel>
            {
                StatusCode = product.StatusCode,
                ErrorMessage = product.ErrorMessage,
                ValidationErrors = string.Join(", ", product.ValidationErrors),
            };
        }

        catch (ProductExceptions<string> exceptions)
        {
            return new ProductResultViewModel<GetProductViewModel>
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
                product.Price,
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

    public async Task<ProductResultViewModel> UpdateProductAsync(int id,
        UpdateProductViewModel product)
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

    public async Task<ProductResultViewModel> DeleteProductAsync(int id,
        DeleteProductViewModel product)
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