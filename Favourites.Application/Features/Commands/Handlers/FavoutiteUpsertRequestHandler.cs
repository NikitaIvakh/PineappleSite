using AutoMapper;
using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Interfaces;
using Favourites.Application.Response;
using Favourites.Domain.Entities.Favourite;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Favourites.Application.Features.Commands.Handlers
{
    public class FavoutiteUpsertRequestHandler(IFavoutiteHeaderDbContext favoutiteHeaderDbContext, IFavoutiteDetailsDbContext favoutiteDetailsDbContext, IMapper mapper) : IRequestHandler<FavoutiteUpsertRequest, FavouriteAPIResponse>
    {
        private readonly IFavoutiteHeaderDbContext _favoutiteHeaderDbContext = favoutiteHeaderDbContext;
        private readonly IFavoutiteDetailsDbContext _favoutiteDetailsDbContext = favoutiteDetailsDbContext;
        private readonly IMapper _mapper = mapper;
        private readonly FavouriteAPIResponse _response = new();

        public async Task<FavouriteAPIResponse> Handle(FavoutiteUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var favouriteHeaderFromDb = await _favoutiteHeaderDbContext.FavouritesHeaders.AsNoTracking().FirstOrDefaultAsync(key => key.UserId == request.Favourites.FavoutiteHeader.UserId, cancellationToken);

                if (favouriteHeaderFromDb is null)
                {
                    FavouritesHeader favouritesHeader = _mapper.Map<FavouritesHeader>(request.Favourites.FavoutiteHeader);
                    await _favoutiteHeaderDbContext.FavouritesHeaders.AddAsync(favouritesHeader, cancellationToken);
                    await _favoutiteHeaderDbContext.SaveChangesAsync(cancellationToken);

                    request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouritesHeader.FavouritesHeaderId;
                    await _favoutiteDetailsDbContext.FavouritesDetails.AddAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()), cancellationToken);
                    await _favoutiteDetailsDbContext.SaveChangesAsync(cancellationToken);
                }

                else
                {
                    var favouriteDetails = await _favoutiteDetailsDbContext.FavouritesDetails
                        .AsNoTracking()
                        .FirstOrDefaultAsync(key => key.ProductId == request.Favourites.FavouritesDetails
                        .First().ProductId && key.FavouritesHeaderId == favouriteHeaderFromDb.FavouritesHeaderId, cancellationToken);

                    if (favouriteDetails is null)
                    {
                        request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouriteHeaderFromDb.FavouritesHeaderId;
                        await _favoutiteDetailsDbContext.FavouritesDetails.AddAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()), cancellationToken);
                        await _favoutiteDetailsDbContext.SaveChangesAsync(cancellationToken);
                    }

                    else
                    {
                        request.Favourites.FavouritesDetails.First().FavouritesDetailsId = favouriteDetails.FavouritesDetailsId;
                        request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouriteDetails.FavouritesHeaderId;

                        _favoutiteDetailsDbContext.FavouritesDetails.Update(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()));
                        await _favoutiteDetailsDbContext.SaveChangesAsync(cancellationToken);
                    }
                }

                _response.IsSuccess = true;
                _response.Data = request.Favourites;

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