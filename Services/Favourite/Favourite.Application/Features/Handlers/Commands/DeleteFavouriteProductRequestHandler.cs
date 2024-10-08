﻿using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Application.Validators;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Commands;

public sealed class DeleteFavouriteProductRequestHandler(
    IBaseRepository<FavouriteHeader> headerRepository,
    IBaseRepository<FavouriteDetails> detailsRepository,
    DeleteValidator deleteValidator,
    IMemoryCache memoryCache) : IRequestHandler<DeleteFavouriteProductRequest, Result<Unit>>
{
    private const string CacheKey = "FavouritesCacheKey";

    public async Task<Result<Unit>> Handle(DeleteFavouriteProductRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var validationResult =
                await deleteValidator.ValidateAsync(request.DeleteFavouriteProductDto, cancellationToken);

            if (!validationResult.IsValid)
            {
                var existErrorMessage = new Dictionary<string, List<string>>()
                {
                    { "Id", validationResult.Errors.Select(key => key.ErrorMessage).ToList() }
                };

                foreach (var error in existErrorMessage)
                {
                    if (existErrorMessage.TryGetValue(error.Key, out var errorMessage))
                    {
                        return new Result<Unit>()
                        {
                            ValidationErrors = errorMessage,
                            StatusCode = (int)StatusCode.NoAction,
                            ErrorMessage =
                                ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture)
                        };
                    }
                }

                return new Result<Unit>()
                {
                    StatusCode = (int)StatusCode.NoAction,
                    ErrorMessage =
                        ErrorMessages.ResourceManager.GetString("ProductCanNotDeleted", ErrorMessages.Culture),
                    ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList(),
                };
            }

            var favouriteProducts =
                detailsRepository.GetAll().Where(key => key.ProductId == request.DeleteFavouriteProductDto.Id).ToList();

            if (favouriteProducts.Count == 0)
            {
                return new Result<Unit>
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

            var favouriteHeaderIds = favouriteProducts.Select(fp => fp.FavouriteHeaderId).Distinct().ToList();

            foreach (var favouriteProduct in favouriteProducts)
            {
                await detailsRepository.DeleteAsync(favouriteProduct);
            }

            foreach (var favouriteHeaderDelete in from favouriteHeaderId in favouriteHeaderIds
                     let totalDetailsWithHeader = detailsRepository.GetAll()
                         .Count(key => key.FavouriteHeaderId == favouriteHeaderId)
                     where totalDetailsWithHeader == 0
                     select headerRepository.GetAll()
                         .FirstOrDefault(key => key.FavouriteHeaderId == favouriteHeaderId))
            {
                if (favouriteHeaderDelete is null)
                {
                    return new Result<Unit>
                    {
                        StatusCode = (int)StatusCode.NotFound,
                        ErrorMessage =
                            ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound",
                                ErrorMessages.Culture),
                        ValidationErrors =
                        [
                            ErrorMessages.ResourceManager.GetString("FavouriteHeaderNotFound",
                                ErrorMessages.Culture) ??
                            string.Empty
                        ]
                    };
                }

                await headerRepository.DeleteAsync(favouriteHeaderDelete);
            }

            memoryCache.Remove(CacheKey);

            return new Result<Unit>
            {
                Data = Unit.Value,
                StatusCode = (int)StatusCode.Deleted,
                SuccessMessage =
                    SuccessMessage.ResourceManager.GetString("ProductSuccessfullyDeleted", SuccessMessage.Culture),
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