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

        public async Task<IReadOnlyCollection<ProductViewModel>> GetAllProductsAsync()
        {
            var products = await _productClient.ProductAllAsync();
            return _mapper.Map<IReadOnlyCollection<ProductViewModel>>(products);
        }

        public async Task<ProductViewModel> GetProductAsync(int id)
        {
            var product = await _productClient.ProductGETAsync(id);
            return _mapper.Map<ProductViewModel>(product);
        }

        public async Task<ProductAPIViewModel> CreateProductAsync(CreateProductViewModel product)
        {
            try
            {
                ProductAPIViewModel response = new();
                CreateProductDto createProductDto = _mapper.Map<CreateProductDto>(product);
                ProductAPIResponse apiREsponse = await _productClient.ProductPOSTAsync(createProductDto);

                if (apiREsponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiREsponse.Message;
                    response.Id = apiREsponse.Id;
                }

                else
                {
                    foreach (string error in apiREsponse.ValidationErrors)
                    {
                        response.Message += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ProductExceptions exception)
            {
                return ConvertProductException(exception);
            }
        }

        public async Task<ProductAPIViewModel> UpdateProductAsync(int id, UpdateProductViewModel product)
        {
            try
            {
                ProductAPIViewModel response = new();
                UpdateProductDto updateProductDto = _mapper.Map<UpdateProductDto>(product);
                ProductAPIResponse apiResponse = await _productClient.ProductPUTAsync(updateProductDto.Id.ToString(), updateProductDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.Message += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ProductExceptions exception)
            {
                return ConvertProductException(exception);
            }
        }

        public async Task<ProductAPIViewModel> DeleteProductAsync(int id, DeleteProductViewModel product)
        {
            try
            {
                ProductAPIViewModel response = new();
                DeleteProductDto deleteProductDto = _mapper.Map<DeleteProductDto>(product);
                ProductAPIResponse apiResponse = await _productClient.ProductDELETEAsync(deleteProductDto.Id.ToString(), deleteProductDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Message = apiResponse.Message;
                    response.Id = apiResponse.Id;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.Message += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (ProductExceptions exception)
            {
                return ConvertProductException(exception);
            }
        }
    }
}