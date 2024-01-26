using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Interfaces;
using Favourites.Application.Response;
using Favourites.Domain.Entities.Favourite;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Application.Features.Commands.Handlers
{
    public class RemoveFavoriteRequestHandler(IFavoutiteHeaderDbContext favoutiteHeaderDbContext, IFavoutiteDetailsDbContext favoutiteDetailsDbContext) : IRequestHandler<RemoveFavoriteRequest, FavouriteAPIResponse>
    {
        private readonly IFavoutiteHeaderDbContext _favoutiteHeaderDbContext = favoutiteHeaderDbContext;
        private readonly IFavoutiteDetailsDbContext _favoutiteDetailsDbContext = favoutiteDetailsDbContext;
        private readonly FavouriteAPIResponse _response = new();

        public async Task<FavouriteAPIResponse> Handle(RemoveFavoriteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouritesDetails favouritesDetails = await _favoutiteDetailsDbContext.FavouritesDetails.FirstOrDefaultAsync(key => key.FavouritesDetailsId == request.FavouriteDetailId, cancellationToken);
                int removeProduct = _favoutiteDetailsDbContext.FavouritesDetails.Where(key => key.FavouritesHeaderId == favouritesDetails.FavouritesHeaderId).Count();
                _favoutiteDetailsDbContext.FavouritesDetails.Remove(favouritesDetails);

                if (removeProduct == 1)
                {
                    FavouritesHeader favouritesHeader = await _favoutiteHeaderDbContext.FavouritesHeaders.FirstOrDefaultAsync(key => key.FavouritesHeaderId == favouritesDetails.FavouritesHeaderId, cancellationToken);
                    _favoutiteHeaderDbContext.FavouritesHeaders.Remove(favouritesHeader);
                }

                await _favoutiteHeaderDbContext.SaveChangesAsync(cancellationToken);
                await _favoutiteDetailsDbContext.SaveChangesAsync(cancellationToken);

                _response.IsSuccess = true;
                _response.Id = request.FavouriteDetailId;
                _response.Data = request.FavouriteDetailId;

                return _response;
            }

            catch (Exception exception)
            {
                _response.IsSuccess = false;
                _response.Message = exception.Message;
            }

            return _response;
        }
    }
}