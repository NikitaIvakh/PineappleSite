using AutoMapper;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Application.Resources;
using Favourites.Domain.DTOs;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Enum;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Domain.Interfaces.Services;
using Favourites.Domain.ResultFavourites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Favourites.Application.Features.Commands.Queries
{
    public class GetFavouriteProductsRequestHandler(IBaseRepository<FavouritesHeader> favouriteHeader, IBaseRepository<FavouritesDetails> favouriteDetails, IMapper mapper,
        ILogger logger, IProductService productService) : IRequestHandler<GetFavouriteProductsRequest, Result<FavouritesDto>>
    {
        private readonly IBaseRepository<FavouritesHeader> _favouriteHeader = favouriteHeader;
        private readonly IBaseRepository<FavouritesDetails> _favouriteDetails = favouriteDetails;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<GetFavouriteProductsRequestHandler>();
        private readonly IProductService _productService = productService;

        public async Task<Result<FavouritesDto>> Handle(GetFavouriteProductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var favouritesHeader = await _favouriteHeader.GetAll().Select(key => new FavoutiteHeaderDto
                {
                    FavouritesHeaderId = key.FavouritesHeaderId,
                    UserId = key.UserId,
                }).FirstOrDefaultAsync(key => key.UserId == request.UserId, cancellationToken);

                if (favouritesHeader is null)
                {
                    return new Result<FavouritesDto>
                    {
                        Data = new FavouritesDto(),
                        SuccessMessage = "Список избранных товаров пуст",
                    };
                }

                var favouritesDetails = _favouriteDetails.GetAll().Select(key => new FavouritesDetailsDto
                {
                    FavouritesDetailsId = key.FavouritesDetailsId,
                    FavouritesHeader = key.FavouritesHeader,
                    FavouritesHeaderId = key.FavouritesHeaderId,
                    Product = key.Product,
                    ProductId = key.ProductId,
                }).Where(key => key.FavouritesHeaderId == favouritesHeader.FavouritesHeaderId).ToList();

                FavouritesDto favouritesDto = new()
                {
                    FavoutiteHeader = _mapper.Map<FavoutiteHeaderDto>(favouritesHeader),
                    FavouritesDetails = new CollectionResult<FavouritesDetailsDto>
                    {
                        Count = favouritesDetails.Count,
                        Data = favouritesDetails,
                        SuccessMessage = "Ваши избранные товары",
                    },
                };

                CollectionResult<ProductDto> productDtos = await _productService.GetProductListAsync();

                foreach (var item in favouritesDto.FavouritesDetails.Data)
                {
                    item.Product = productDtos.Data.FirstOrDefault(key => key.Id == item.ProductId);
                }

                return new Result<FavouritesDto>
                {
                    Data = favouritesDto,
                    SuccessMessage = "Ваши избранные товары",
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<FavouritesDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}