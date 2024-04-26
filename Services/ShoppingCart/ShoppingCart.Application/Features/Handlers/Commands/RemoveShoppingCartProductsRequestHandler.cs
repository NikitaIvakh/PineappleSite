using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Application.Validators;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class RemoveShoppingCartProductsRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    DeleteProductsValidator deleteProductsValidator,
    IMemoryCache memoryCache) : IRequestHandler<RemoveShoppingCartProductsRequest, CollectionResult<bool>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<CollectionResult<bool>> Handle(RemoveShoppingCartProductsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validarionResult =
                await deleteProductsValidator.ValidateAsync(request.DeleteProductDto, cancellationToken);

            if (!validarionResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "ProductIds", validarionResult.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new CollectionResult<bool>()
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture)
                        };
                    }
                }

                return new CollectionResult<bool>()
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductsCanNotDeleted", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductsCanNotDeleted", ErrorMessages.Culture) ??
                        string.Empty
                    ],
                };
            }

            var cartDetails = cartDetailsRepository.GetAll()
                .Where(key => request.DeleteProductDto.ProductIds.Contains(key.ProductId)).ToList();

            if (cartDetails.Count == 0)
            {
                return new CollectionResult<bool>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("DetailsNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("DetailsNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            foreach (var product in cartDetails)
            {
                await cartDetailsRepository.DeleteAsync(product);
            }

            var cartHearerIds = cartDetails.Select(cd => cd.CartHeaderId).Distinct().ToList();

            foreach (var cartHeaderFromDb in from cartHeaderId in cartHearerIds
                     let totalDetailsWithHeader =
                         cartDetailsRepository.GetAll().Count(key => key.CartHeaderId == cartHeaderId)
                     where totalDetailsWithHeader == 1
                     select cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartHeaderId))
            {
                if (cartHeaderFromDb is null)
                {
                    return new CollectionResult<bool>()
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage =
                            ErrorMessages.ResourceManager.GetString("CartHeaderNotFound",
                                ErrorMessages.Culture),
                        ValidationErrors =
                        [
                            ErrorMessages.ResourceManager.GetString("CartHeaderNotFound",
                                ErrorMessages.Culture) ??
                            string.Empty
                        ]
                    };
                }

                await cartHeaderRepository.DeleteAsync(cartHeaderFromDb);
            }

            memoryCache.Remove(CacheKey);

            return new CollectionResult<bool>
            {
                Data = [true],
                Count = cartDetails.Count,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage = SuccessMessage.ProductsSuccessfullyDeleted,
            };
        }

        catch (Exception ex)
        {
            return new CollectionResult<bool>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}