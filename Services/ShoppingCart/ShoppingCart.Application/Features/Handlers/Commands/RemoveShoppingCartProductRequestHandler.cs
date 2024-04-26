﻿using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class RemoveShoppingCartProductRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    IMemoryCache memoryCache) : IRequestHandler<RemoveShoppingCartProductRequest, Result<Unit>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<Unit>> Handle(RemoveShoppingCartProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var cartDetails =
                cartDetailsRepository.GetAll().Where(key => key.ProductId == request.ProductId).ToList();

            if (cartDetails.Count == 0)
            {
                return new Result<Unit>
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

            var cartHeaderIds = cartDetails.Select(cd => cd.CartHeaderId).Distinct().ToList();

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

                await cartHeaderRepository.DeleteAsync(cartHeaderDelete);
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