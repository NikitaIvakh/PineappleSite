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
    public class DeleteProductListRequestHamdler(IBaseRepositiry<FavouriteHeader> favouriteHeaderRepository, IBaseRepositiry<FavouriteDetails> favouriteDetailsRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<DeleteProductListRequest, Result<FavouriteHeaderDto>>
    {
        private readonly IBaseRepositiry<FavouriteHeader> _favouriteHeaderRepository = favouriteHeaderRepository;
        private readonly IBaseRepositiry<FavouriteDetails> _favouriteDetailsRepository = favouriteDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "FavouritesCacheKey";

        public async Task<Result<FavouriteHeaderDto>> Handle(DeleteProductListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var favouriteDetails = _favouriteDetailsRepository.GetAll().Where(key => request.DeleteFavourite.ProductIds.Contains(key.ProductId)).ToList();
                var favouriteHeader = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FirstOrDefault().FavouriteHeaderId);

                if (favouriteDetails is null || favouriteDetails.Count == 0)
                {
                    return new Result<FavouriteHeaderDto>
                    {
                        Data = new FavouriteHeaderDto(),
                        ErrorCode = (int)ErrorCodes.ProductsNotFound,
                        ErrorMessage = ErrorMessages.ProductsNotFound,
                        ValidationErrors = [ErrorMessages.ProductsNotFound]
                    };
                }

                else
                {
                    int totalRemoveProducts = _favouriteDetailsRepository.GetAll().Where(key => key.FavouriteHeaderId == favouriteDetails.FirstOrDefault().FavouriteHeaderId).Count();
                    await _favouriteDetailsRepository.DeleteListAsync(favouriteDetails);

                    if (totalRemoveProducts == 1)
                    {
                        var favouriteHeaderFromDb = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FirstOrDefault().FavouriteHeaderId);

                        if (favouriteHeaderFromDb is not null)
                        {
                            await _favouriteHeaderRepository.DeleteAsync(favouriteHeaderFromDb);
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
                        SuccessMessage = SuccessMessage.ProductsSuccessfullyDeleted,
                    };
                }
            }

            catch (Exception exception)
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