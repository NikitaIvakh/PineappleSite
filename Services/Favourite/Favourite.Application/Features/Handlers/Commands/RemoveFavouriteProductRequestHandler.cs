using AutoMapper;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Domain.DTOs;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Commands
{
    public class RemoveFavouriteProductRequestHandler(IBaseRepositiry<FavouriteHeader> headerRepository, IBaseRepositiry<FavouriteDetails> detailsRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<RemoveFavouriteProductRequest, Result<FavouriteHeaderDto>>
    {
        private readonly IBaseRepositiry<FavouriteHeader> _favouriteHeaderRepository = headerRepository;
        private readonly IBaseRepositiry<FavouriteDetails> _favouriteDetailsRepository = detailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "FavouritesCacheKey";

        public async Task<Result<FavouriteHeaderDto>> Handle(RemoveFavouriteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouriteDetails? favouriteDetails = _favouriteDetailsRepository.GetAll().FirstOrDefault(key => key.ProductId == request.ProductId);
                FavouriteHeader? favouriteHeader = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId);

                if (favouriteDetails is null)
                {
                    return new Result<FavouriteHeaderDto>
                    {
                        ErrorCode = (int)ErrorCodes.ProductNotFound,
                        ErrorMessage = ErrorMessages.ProductNotFound,
                        ValidationErrors = [ErrorMessages.ProductNotFound]
                    };
                }

                else
                {
                    int totalRemoveFavouriteItems = _favouriteDetailsRepository.GetAll().Where(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId).Count();
                    await _favouriteDetailsRepository.DeleteAsync(favouriteDetails);

                    if (totalRemoveFavouriteItems == 1)
                    {
                        FavouriteHeader? favouriteHeaderDelete = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId);

                        if (favouriteHeaderDelete is null)
                        {
                            return new Result<FavouriteHeaderDto>
                            {
                                ErrorCode = (int)ErrorCodes.FavouriteHeaderNotFound,
                                ErrorMessage = ErrorMessages.FavouriteHeaderNotFound,
                                ValidationErrors = [ErrorMessages.FavouriteHeaderNotFound]
                            };
                        }

                        else if (favouriteHeaderDelete is not null)
                        {
                            await _favouriteHeaderRepository.DeleteAsync(favouriteHeaderDelete);
                        }
                    }

                    var getAllheaders = _favouriteHeaderRepository.GetAll().ToList();
                    var getAlldetails = _favouriteDetailsRepository.GetAll().ToList();

                    _memoryCache.Remove(getAllheaders);
                    _memoryCache.Remove(getAlldetails);

                    _memoryCache.Set(cacheKey, getAllheaders);
                    _memoryCache.Set(cacheKey, getAlldetails);

                    return new Result<FavouriteHeaderDto>
                    {
                        SuccessCode = (int)SuccessCode.Deleted,
                        Data = _mapper.Map<FavouriteHeaderDto>(favouriteHeader),
                        SuccessMessage = SuccessMessage.ProductSuccessfullyDeleted,
                    };
                }
            }

            catch
            {
                return new Result<FavouriteHeaderDto>
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ValidationErrors = [ErrorMessages.InternalServerError]
                };
            }
        }
    }
}