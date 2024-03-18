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
    public class FavouriteProductUpsertRequestHandler(IBaseRepositiry<FavouriteHeader> headerRepository, IBaseRepositiry<FavouriteDetails> detailsRepository, IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<FavouriteProductUpsertRequest, Result<FavouriteHeaderDto>>
    {
        private readonly IBaseRepositiry<FavouriteHeader> _favouriteHeaderRepository = headerRepository;
        private readonly IBaseRepositiry<FavouriteDetails> _favouriteDetailsRepository = detailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "FavouritesCacheKey";

        public async Task<Result<FavouriteHeaderDto>> Handle(FavouriteProductUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouriteHeader? favouriteHeaderFromDb = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.UserId == request.FavouriteDto.FavouriteHeader.UserId);

                if (favouriteHeaderFromDb is null)
                {
                    FavouriteHeader? favouriteHeader = _mapper.Map<FavouriteHeader>(request.FavouriteDto.FavouriteHeader);
                    await _favouriteHeaderRepository.CreateAsync(favouriteHeader);

                    request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId = favouriteHeader.FavouriteHeaderId;
                    await _favouriteDetailsRepository.CreateAsync(_mapper.Map<FavouriteDetails>(request.FavouriteDto.FavouriteDetails.First()));

                    var getAllheaders = _favouriteHeaderRepository.GetAll().ToList();
                    var getAlldetails = _favouriteDetailsRepository.GetAll().ToList();

                    _memoryCache.Remove(getAllheaders);
                    _memoryCache.Remove(getAlldetails);

                    _memoryCache.Set(cacheKey, getAllheaders);
                    _memoryCache.Set(cacheKey, getAlldetails);

                    return new Result<FavouriteHeaderDto>
                    {
                        SuccessMessage = "Продукт успешно добавлен в избранное",
                        Data = _mapper.Map<FavouriteHeaderDto>(favouriteHeader),
                    };
                }

                else
                {
                    FavouriteDetails? favouriteDetails = _favouriteDetailsRepository
                        .GetAll()
                        .FirstOrDefault(key => key.ProductId == request.FavouriteDto.FavouriteDetails.First().ProductId && key.FavouriteHeaderId == favouriteHeaderFromDb.FavouriteHeaderId);

                    if (favouriteDetails is null)
                    {
                        request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId = favouriteHeaderFromDb.FavouriteHeaderId;
                        await _favouriteDetailsRepository.CreateAsync(_mapper.Map<FavouriteDetails>(request.FavouriteDto.FavouriteDetails.First()));
                    }

                    else
                    {
                        request.FavouriteDto.FavouriteDetails.First().FavouriteHeaderId = favouriteDetails.FavouriteHeaderId;
                        request.FavouriteDto.FavouriteDetails.First().FavouriteDetailsId = favouriteDetails.FavouriteDetailsId;
                    }

                    return new Result<FavouriteHeaderDto>
                    {
                        SuccessMessage = "Продукт успешно добавлен в избранное",
                        Data = _mapper.Map<FavouriteHeaderDto>(favouriteHeaderFromDb),
                    };
                }
            }

            catch (Exception exception)
            {
                return new Result<FavouriteHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}