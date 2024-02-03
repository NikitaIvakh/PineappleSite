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
    public class FavoutiteUpsertRequestHandler(IBaseRepository<FavouritesHeader> favouriteHeader, IBaseRepository<FavouritesDetails> favouriteDetails, IMapper mapper, ILogger logger) : IRequestHandler<FavoutiteUpsertRequest, Result<FavouritesHeaderDto>>
    {
        private readonly IBaseRepository<FavouritesHeader> _favouriteHeader = favouriteHeader;
        private readonly IBaseRepository<FavouritesDetails> _favouriteDetails = favouriteDetails;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<FavoutiteUpsertRequestHandler>();

        public async Task<Result<FavouritesHeaderDto>> Handle(FavoutiteUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var favouriteHeaderFromDb = await _favouriteHeader.GetAll().FirstOrDefaultAsync(key => key.UserId == request.Favourites.FavoutiteHeader.UserId, cancellationToken);

                if (favouriteHeaderFromDb is null)
                {
                    FavouritesHeader favouritesHeader = _mapper.Map<FavouritesHeader>(request.Favourites.FavoutiteHeader);
                    await _favouriteHeader.CreateAsync(favouritesHeader);

                    request.Favourites.FavouritesDetails.Data.First().FavouritesHeaderId = favouritesHeader.FavouritesHeaderId;
                    await _favouriteDetails.CreateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.Data.First()));

                    return new Result<FavouritesHeaderDto>
                    {
                        Data = _mapper.Map<FavouritesHeaderDto>(favouritesHeader),
                        SuccessMessage = "Продукт успешно добавлен в избранное",
                    };
                }

                else
                {
                    var favouriteDetails = await _favouriteDetails
                        .GetAll()
                        .FirstOrDefaultAsync(key => key.ProductId == request.Favourites.FavouritesDetails.Data
                        .First().ProductId && key.FavouritesHeaderId == favouriteHeaderFromDb.FavouritesHeaderId, cancellationToken);

                    if (favouriteDetails is null)
                    {
                        request.Favourites.FavouritesDetails.Data.First().FavouritesHeaderId = favouriteHeaderFromDb.FavouritesHeaderId;
                        await _favouriteDetails.CreateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.Data.First()));
                    }

                    else
                    {
                        request.Favourites.FavouritesDetails.Data.First().FavouritesDetailsId = favouriteDetails.FavouritesDetailsId;
                        request.Favourites.FavouritesDetails.Data.First().FavouritesHeaderId = favouriteDetails.FavouritesHeaderId;

                        await _favouriteDetails.UpdateAsync(_mapper.Map<FavouritesDetails>(request.Favourites.FavouritesDetails.Data.First()));
                    }
                }

                return new Result<FavouritesHeaderDto>
                {
                    Data = _mapper.Map<FavouritesHeaderDto>(favouriteHeaderFromDb),
                    SuccessMessage = "Продукт успешно добавлен в избранное",
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<FavouritesHeaderDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}