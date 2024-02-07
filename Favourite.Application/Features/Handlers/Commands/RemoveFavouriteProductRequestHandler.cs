using AutoMapper;
using Favourite.Application.Features.Requests.Commands;
using Favourite.Application.Resources;
using Favourite.Domain.DTOs;
using Favourite.Domain.Entities;
using Favourite.Domain.Enum;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Results;
using MediatR;

namespace Favourite.Application.Features.Handlers.Commands
{
    public class RemoveFavouriteProductRequestHandler(IBaseRepositiry<FavouriteHeader> headerRepository, IBaseRepositiry<FavouriteDetails> detailsRepository, IMapper mapper) : IRequestHandler<RemoveFavouriteProductRequest, Result<FavouriteHeaderDto>>
    {
        private readonly IBaseRepositiry<FavouriteHeader> _favouriteHeaderRepository = headerRepository;
        private readonly IBaseRepositiry<FavouriteDetails> _favouriteDetailsRepository = detailsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<FavouriteHeaderDto>> Handle(RemoveFavouriteProductRequest request, CancellationToken cancellationToken)
        {
            try
            {
                FavouriteDetails? favouriteDetails = _favouriteDetailsRepository.GetAll().FirstOrDefault(key => key.ProductId == request.ProductId);
                FavouriteHeader? favouriteHeader = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId);

                if (favouriteDetails is null)
                {
                    return new Result<FavouriteHeaderDto>
                    {
                        ErrorMessage = ErrorMessages.InternalServerError,
                        ErrorCode = (int)ErrorCodes.InternalServerError,
                    };
                }

                else
                {
                    int totalRemoveFavouriteItems = _favouriteDetailsRepository.GetAll().Where(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId).Count();
                    await _favouriteDetailsRepository.DeleteAsync(favouriteDetails);

                    if (totalRemoveFavouriteItems == 1)
                    {
                        FavouriteHeader? favouriteHeaderDelete = _favouriteHeaderRepository.GetAll().FirstOrDefault(key => key.FavouriteHeaderId == favouriteDetails.FavouriteHeaderId);

                        if (favouriteHeaderDelete is not null)
                        {
                            await _favouriteHeaderRepository.DeleteAsync(favouriteHeaderDelete);
                        }
                    }

                    return new Result<FavouriteHeaderDto>
                    {
                        SuccessMessage = "Продукт успешно удален",
                        Data = _mapper.Map<FavouriteHeaderDto>(favouriteHeader),
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