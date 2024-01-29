using AutoMapper;
using Favourites.Application.Features.Requests.Handlers;
using Favourites.Application.Resources;
using Favourites.Domain.DTOs;
using Favourites.Domain.Entities.Favourite;
using Favourites.Domain.Enum;
using Favourites.Domain.Interfaces.Repositories;
using Favourites.Domain.ResultFavourites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Favourites.Application.Features.Commands.Handlers
{
    public class FavoutiteUpsertRequestHandler(IBaseRepository<FavouritesHeader> favouriteHeader, IBaseRepository<FavouritesDetails> favouriteDetails, IMapper mapper, ILogger logger) : IRequestHandler<FavoutiteUpsertRequest, Result<FavouritesDto>>
    {
        private readonly IBaseRepository<FavouritesHeader> _favouriteHeader = favouriteHeader;
        private readonly IBaseRepository<FavouritesDetails> _favouriteDetails = favouriteDetails;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;

        public async Task<Result<FavouritesDto>> Handle(FavoutiteUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var favouriteHeaderFromDb = await _favouriteHeader.GetAll().FirstOrDefaultAsync(key => key.UserId == request.Favourites.FavoutiteHeader.UserId, cancellationToken);

                if (favouriteHeaderFromDb is null)
                {
                    FavouritesHeader favouritesHeader = _mapper.Map<FavouritesHeader>(request.Favourites.FavoutiteHeader);
                    await _favouriteHeader.CreateAsync(favouritesHeader);

                    request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouritesHeader.FavouritesHeaderId;
                    await _favouriteDetails.CreateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()));
                }

                else
                {
                    var favouriteDetails = await _favouriteDetails
                        .GetAll()
                        .FirstOrDefaultAsync(key => key.ProductId == request.Favourites.FavouritesDetails
                        .First().ProductId && key.FavouritesHeaderId == favouriteHeaderFromDb.FavouritesHeaderId, cancellationToken);

                    if (favouriteDetails is null)
                    {
                        request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouriteHeaderFromDb.FavouritesHeaderId;
                        await _favouriteDetails.CreateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()));
                    }

                    else
                    {
                        request.Favourites.FavouritesDetails.First().FavouritesDetailsId = favouriteDetails.FavouritesDetailsId;
                        request.Favourites.FavouritesDetails.First().FavouritesHeaderId = favouriteDetails.FavouritesHeaderId;

                        await _favouriteDetails.UpdateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.First()));
                    }
                }

                return new Result<FavouritesDto>
                {
                    Data = request.Favourites,
                    SuccessMessage = "Продукт успешно добавлен в избранное",
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