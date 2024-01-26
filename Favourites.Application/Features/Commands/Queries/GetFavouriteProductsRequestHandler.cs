using AutoMapper;
using Favourites.Application.DTOs;
using Favourites.Application.Features.Requests.Queries;
using Favourites.Application.Interfaces;
using Favourites.Application.Response;
using Favourites.Application.Services.IServices;
using Favourites.Domain.Entities.Favourite;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Application.Features.Commands.Queries
{
    public class GetFavouriteProductsRequestHandler(IFavoutiteHeaderDbContext favoutiteHeaderDbContext, IFavoutiteDetailsDbContext favoutiteDetailsDbContext, IMapper mapper,
        IProductService productService) : IRequestHandler<GetFavouriteProductsRequest, FavouriteAPIResponse>
    {
        private readonly IFavoutiteHeaderDbContext _favoutiteHeaderDbContext = favoutiteHeaderDbContext;
        private readonly IFavoutiteDetailsDbContext _favoutiteDetailsDbContext = favoutiteDetailsDbContext;
        private readonly IMapper _mapper = mapper;
        private readonly IProductService _productService = productService;
        private readonly FavouriteAPIResponse _response = new();

        public async Task<FavouriteAPIResponse> Handle(GetFavouriteProductsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouritesHeader favouritesHeader = await _favoutiteHeaderDbContext.FavouritesHeaders.AsNoTracking().FirstOrDefaultAsync(key => key.UserId == request.UserId, cancellationToken);

                if (favouritesHeader == null)
                {
                    _response.IsSuccess = true;
                    _response.Data = new FavouritesHeader();

                    return _response;
                }

                var favouritesDetails = await _favoutiteDetailsDbContext.FavouritesDetails.Where(key => key.FavouritesHeaderId == favouritesHeader.FavouritesHeaderId).ToListAsync(cancellationToken);

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

                _response.IsSuccess = true;
                _response.Data = favouritesDto;
                return _response;
            }

            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Message = exception.Message;
            }

            _response.IsSuccess = true;
            return _response;
        }
    }
}