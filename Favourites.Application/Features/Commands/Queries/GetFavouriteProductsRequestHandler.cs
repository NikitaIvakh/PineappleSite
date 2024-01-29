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
        private readonly ILogger _logger = logger;
        private readonly IProductService _productService = productService;

        public async Task<Result<FavouritesDto>> Handle(GetFavouriteProductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouritesHeader favouritesHeader = await _favouriteHeader.GetAll().FirstOrDefaultAsync(key => key.UserId == request.UserId, cancellationToken);

                if (favouritesHeader is null)
                {
                    return new Result<FavouritesDto>
                    {
                        Data = new FavouritesDto(),
                        SuccessMessage = "Список избранных товаров пуст",
                    };
                }

                var favouritesDetails = await _favouriteDetails.GetAll().Where(key => key.FavouritesHeaderId == favouritesHeader.FavouritesHeaderId).ToListAsync(cancellationToken);

                FavouritesDto favouritesDto = new()
                {
                    FavoutiteHeader = _mapper.Map<FavoutiteHeaderDto>(favouritesHeader),
                    FavouritesDetails = _mapper.Map<IReadOnlyCollection<FavouritesDetailsDto>>(favouritesDetails),
                };

                IReadOnlyCollection<ProductDto> productDtos = await _productService.GetProductListAsync();

                foreach (var item in favouritesDto.FavouritesDetails)
                {
                    item.Product = productDtos.FirstOrDefault(key => key.Id == item.ProductId);
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