using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Resources;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Enum;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Domain.ResultFavourites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Favourites.Application.Features.Commands.Handlers
{
    public class RemoveFavoriteRequestHandler(IBaseRepository<FavouritesHeader> favouriteHeader, IBaseRepository<FavouritesDetails> favouriteDetails, ILogger logger) : IRequestHandler<RemoveFavoriteRequest, Result<FavouritesDetails>>
    {
        private readonly IBaseRepository<FavouritesHeader> _favouriteHeader = favouriteHeader;
        private readonly IBaseRepository<FavouritesDetails> _favouriteDetails = favouriteDetails;
        private readonly ILogger _logger = logger;

        public async Task<Result<FavouritesDetails>> Handle(RemoveFavoriteRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouritesDetails? favouritesDetails = await _favouriteDetails.GetAll().FirstOrDefaultAsync(key => key.FavouritesDetailsId == request.FavouriteDetailId, cancellationToken);
                int removeProduct = await _favouriteDetails.GetAll().Where(key => key.FavouritesHeaderId == favouritesDetails.FavouritesHeaderId).CountAsync(cancellationToken);
                await _favouriteDetails.DeleteAsync(favouritesDetails);

                if (removeProduct == 1)
                {
                    FavouritesHeader? favouritesHeader = await _favouriteHeader.GetAll().FirstOrDefaultAsync(key => key.FavouritesHeaderId == favouritesDetails.FavouritesHeaderId, cancellationToken);

                    if (favouritesHeader is not null)
                        await _favouriteHeader.DeleteAsync(favouritesHeader);
                }

                return new Result<FavouritesDetails>
                {
                    Data = favouritesDetails,
                    SuccessMessage = "Продукт успешно удален",
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<FavouritesDetails>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}