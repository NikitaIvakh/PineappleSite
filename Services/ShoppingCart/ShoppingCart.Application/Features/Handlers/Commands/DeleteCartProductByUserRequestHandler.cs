﻿using MediatR;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Application.Validators;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands;

public sealed class DeleteCartProductByUserRequestHandler(
    IBaseRepository<CartHeader> cartHeaderRepository,
    IBaseRepository<CartDetails> cartDetailsRepository,
    DeleteValidator deleteValidator,
    IMemoryCache memoryCache)
    : IRequestHandler<DeleteCartProductByUserRequest, Result<Unit>>
{
    private const string CacheKey = "cacheGetShoppingCartKey";

    public async Task<Result<Unit>> Handle(DeleteCartProductByUserRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var validatorResult = await deleteValidator.ValidateAsync(request.DeleteProductDto, cancellationToken);

            if (!validatorResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "ProductId", validatorResult.Errors.Select(key => key.ErrorMessage).ToList() }
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
                                ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture)
                        };
                    }
                }

                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NoContent,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture) ??
                        string.Empty
                    ],
                };
            }

            var cartDetail = cartDetailsRepository.GetAll()
                .FirstOrDefault(key =>
                    key.ProductId == request.DeleteProductDto.ProductId &&
                    key.CartHeader!.UserId == request.UserId);

            if (cartDetail is null)
            {
                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NotFound,
                    ErrorMessage = ErrorMessages.ResourceManager.GetString("ProductNotFound", ErrorMessages.Culture),
                    ValidationErrors =
                    [
                        ErrorMessages.ResourceManager.GetString("ProductNotFound", ErrorMessages.Culture) ??
                        string.Empty
                    ]
                };
            }

            await cartDetailsRepository.DeleteAsync(cartDetail);

            var getDeleteProductsCount = cartDetailsRepository.GetAll()
                .Count(key => key.CartHeaderId == cartDetail.CartHeaderId);

            if (getDeleteProductsCount == 1)
            {
                var cartHeader = cartHeaderRepository.GetAll().FirstOrDefault(key =>
                    key.CartHeaderId == cartDetail.CartHeaderId &&
                    key.UserId == cartDetail.CartHeader!.UserId)!;

                await cartHeaderRepository.DeleteAsync(cartHeader);
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>()
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture)
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