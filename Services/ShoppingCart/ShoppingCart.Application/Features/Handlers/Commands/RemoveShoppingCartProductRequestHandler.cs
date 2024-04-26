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

public sealed class RemoveShoppingCartProductRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    DeleteValidator deleteValidator,
    IMemoryCache memoryCache) : IRequestHandler<RemoveShoppingCartProductRequest, Result<Unit>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<Unit>> Handle(RemoveShoppingCartProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult = await deleteValidator.ValidateAsync(request.DeleteProductDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "ProductId", validationResult.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>()
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoContent,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                        };
                    }
                }

                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList(),
                };
            }

            var cartDetails =
                cartDetailsRepository.GetAll().Where(key => key.ProductId == request.DeleteProductDto.Id).ToList();

            if (cartDetails.Count == 0)
            {
                return new Result<Unit>
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductsNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            var cartHeaderIds = cartDetails.Select(cd => cd.CartHeaderId).Distinct().ToList();
            
            foreach (var product in cartDetails)
            {
                await cartDetailsRepository.DeleteAsync(product);
            }

            foreach (var cartHeaderDelete in from cartHeaderId in cartHeaderIds
                     let totalDetailsWithHeader = cartDetailsRepository.GetAll().Count(key => key.CartHeaderId == cartHeaderId)
                     where totalDetailsWithHeader == 1
                     select cartHeaderRepository.GetAll().FirstOrDefault(key => key.CartHeaderId == cartHeaderId))
            {
                if (cartHeaderDelete is null)
                {
                    return new Result<Unit>
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

                var usersWithProductsCount = cartHeaderRepository.GetAll()
                    .GroupBy(key => key.UserId)
                    .Select(group => new { UserId = group.Key, ProductCount = group.Count() });
                
                foreach (var user in usersWithProductsCount)
                {
                    if (user.ProductCount == 1 )
                    {
                        var cartHeader = cartHeaderRepository.GetAll()
                            .FirstOrDefault(key => key.UserId == user.UserId);

                        if (cartHeader is not null)
                        {
                            await cartHeaderRepository.DeleteAsync(cartHeaderDelete);
                            await cartHeaderRepository.DeleteAsync(cartHeader);
                        }
                    }
                    
                    await cartHeaderRepository.DeleteAsync(cartHeaderDelete);
                }
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted",
                        SuccessMessage.Culture),
            };
        }

        catch (Exception ex)
        {
            return new Result<Unit>
            {
                ErrorMessage = ex.Message,
                ValidationErrors = [ex.Message],
                StatusCode = (int)StatusCode.InternalServerError,
            };
        }
    }
}