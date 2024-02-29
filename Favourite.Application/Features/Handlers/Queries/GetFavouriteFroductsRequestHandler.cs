using AutoMapper;
using Favourite.Application.Features.Requests.Queries;
using Favourite.Application.Resources;
using Favourite.Domain.DTOs;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Domain.Results;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Application.Features.Handlers.Queries
{
    public class GetFavouriteFroductsRequestHandler(IBaseRepositiry<FavouriteHeader> _headerRepository, IBaseRepositiry<FavouriteDetails> detailsRepository, IProductService productService,
        IMapper mapper, IMemoryCache memoryCache) : IRequestHandler<GetFavouriteFroductsRequest, Result<FavouriteDto>>
    {
        private readonly IBaseRepositiry<FavouriteHeader> _favouriteHeaderRepository = _headerRepository;
        private readonly IBaseRepositiry<FavouriteDetails> _favouriteDetailsRepository = detailsRepository;
        private readonly IProductService _productService = productService;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _memoryCache = memoryCache;

        private readonly string cacheKey = "cacheGetFavouritekey";

        public async Task<Result<FavouriteDto>> Handle(GetFavouriteFroductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out FavouriteDto? favouriteDto))
                {
                    return new Result<FavouriteDto>
                    {
                        Data = favouriteDto,
                        SuccessMessage = "Ваши избранные товары",
                    };
                }

                else
                {
                    FavouriteHeaderDto? favouriteHeader = _favouriteHeaderRepository.GetAll().Select(key => new FavouriteHeaderDto
                    {
                        FavouriteHeaderId = key.FavouriteHeaderId,
                        UserId = key.UserId,
                    }).FirstOrDefault(key => key.UserId == request.UserId);

                    if (favouriteHeader is null)
                    {
                        return new Result<FavouriteDto>
                        {
                            Data = new FavouriteDto()
                            {
                                FavouriteHeader = new FavouriteHeaderDto(),
                                FavouriteDetails = new List<FavouriteDetailsDto>(),
                            },

                            SuccessMessage = "В избранном никаких товаров нет",
                        };
                    }

                    else
                    {
                        List<FavouriteDetailsDto> favouriteDetails = _favouriteDetailsRepository.GetAll().Select(key => new FavouriteDetailsDto
                        {
                            FavouriteDetailsId = key.FavouriteDetailsId,
                            FavouriteHeader = _mapper.Map<FavouriteHeaderDto>(key.FavouriteHeader),
                            FavouriteHeaderId = key.FavouriteHeaderId,
                            Product = key.Product,
                            ProductId = key.ProductId,
                        }).OrderByDescending(key => key.FavouriteDetailsId).ToList();

                        if (favouriteDetails is null || favouriteDetails.Count == 0)
                        {
                            return new Result<FavouriteDto>
                            {
                                Data = new FavouriteDto()
                                {
                                    FavouriteHeader = new FavouriteHeaderDto(),
                                    FavouriteDetails = new List<FavouriteDetailsDto>(),
                                },

                                SuccessMessage = "В избранном никаких товаров нет",
                            };
                        }

                        else
                        {
                            favouriteDto = new()
                            {
                                FavouriteHeader = favouriteHeader,
                                FavouriteDetails = favouriteDetails,
                            };

                            CollectionResult<ProductDto> products = await _productService.GetProductListAsync();

                            foreach (var item in favouriteDto.FavouriteDetails)
                            {
                                item.Product = products?.Data?.FirstOrDefault(key => key.Id == item.ProductId);
                            }

                            return new Result<FavouriteDto>
                            {
                                Data = favouriteDto,
                                SuccessMessage = "Ваши избранные товары",
                            };
                        }
                    }
                }
            }

            catch (Exception exception)
            {
                return new Result<FavouriteDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}