using AutoMapper;
using MediatR;
using ShoppingCart.Application.Features.Requests.Commands;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enum;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Results;

namespace ShoppingCart.Application.Features.Handlers.Commands
{
    public class ShoppingCartUpsertRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper) : IRequestHandler<ShoppingCartUpsertRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<CartHeaderDto>> Handle(ShoppingCartUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeaderFromDb = _cartHeaderRepository.GetAll().FirstOrDefault(key => key.UserId == request.CartDto.CartHeader.UserId);

                if (cartHeaderFromDb is null)
                {
                    CartHeader? cartHeader = _mapper.Map<CartHeader>(request.CartDto.CartHeader);
                    await _cartHeaderRepository.CreateAsync(cartHeader);

                    request.CartDto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    await _cartDetailsRepository.CreateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
                }

                else
                {
                    var cartDetailsFromDb = _cartDetailsRepository
                        .GetAll()
                        .FirstOrDefault(key => key.ProductId == request.CartDto.CartDetails.First().ProductId && key.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if (cartDetailsFromDb is null)
                    {
                        request.CartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        await _cartDetailsRepository.CreateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
                    }

                    else
                    {
                        request.CartDto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        request.CartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        request.CartDto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;

                        await _cartDetailsRepository.UpdateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.First()));
                    }
                }

                return new Result<CartHeaderDto>
                {
                    Data = _mapper.Map<CartHeaderDto>(cartHeaderFromDb),
                    SuccessMessage = "Товар успешно добавлен в корзину",
                };
            }

            catch (Exception exception)
            {
                return new Result<CartHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ValidationErrors = new List<string> { exception.Message }
                };
            }
        }
    }
}