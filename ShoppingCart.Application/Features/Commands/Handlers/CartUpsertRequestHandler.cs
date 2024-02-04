using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Application.Features.Requests.Handlers;
using ShoppingCart.Domain.Entities.Cart;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.ResultCart;
using ShoppingCart.Domain.DTOs;
using Serilog;
using ShoppingCart.Application.Resources;
using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Application.Features.Commands.Handlers
{
    public class CartUpsertRequestHandler(IBaseRepository<CartHeader> cartHeaderRepository, IBaseRepository<CartDetails> cartDetailsRepository, IMapper mapper, ILogger logger) : IRequestHandler<CartUpsertRequest, Result<CartHeaderDto>>
    {
        private readonly IBaseRepository<CartHeader> _cartHeaderRepository = cartHeaderRepository;
        private readonly IBaseRepository<CartDetails> _cartDetailsRepository = cartDetailsRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger.ForContext<CartUpsertRequestHandler>();

        public async Task<Result<CartHeaderDto>> Handle(CartUpsertRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var cartHeaderFromDb = await _cartHeaderRepository.GetAll().FirstOrDefaultAsync(key => key.UserId == request.CartDto.CartHeader.UserId, cancellationToken);

                if (cartHeaderFromDb is null)
                {
                    CartHeader cartHeader = _mapper.Map<CartHeader>(request.CartDto.CartHeader);
                    await _cartHeaderRepository.CreateAsync(cartHeader);

                    request.CartDto.CartDetails.Data.First().CartHeaderId = cartHeader.Id;
                    await _cartDetailsRepository.CreateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.Data.First()));

                    return new Result<CartHeaderDto>
                    {
                        Data = _mapper.Map<CartHeaderDto>(cartHeader),
                        SuccessMessage = "Продукт успешно добавлен в корзину",
                    };
                }

                else
                {
                    var cartDetailsFromDb = await _cartDetailsRepository.GetAll().FirstOrDefaultAsync(key => key.ProductId == request.CartDto.CartDetails.Data.First().ProductId &&
                        key.CartHeaderId == cartHeaderFromDb.Id, cancellationToken);

                    if (cartDetailsFromDb is null || cartDetailsFromDb.Count == 0)
                    {
                        request.CartDto.CartDetails.Data.First().CartHeaderId = cartHeaderFromDb.Id;
                        await _cartDetailsRepository.CreateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.Data.First()));
                    }

                    else
                    {
                        request.CartDto.CartDetails.Data.First().Id = cartDetailsFromDb.Id;
                        request.CartDto.CartDetails.Data.First().Count += cartDetailsFromDb.Count;
                        request.CartDto.CartDetails.Data.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;

                        await _cartDetailsRepository.UpdateAsync(_mapper.Map<CartDetails>(request.CartDto.CartDetails.Data.First()));
                    }
                }

                return new Result<CartHeaderDto>
                {
                    Data = _mapper.Map<CartHeaderDto>(cartHeaderFromDb),
                    SuccessMessage = "Корзина успешно обновлена",
                };
            }

            catch (Exception exception)
            {
                _logger.Warning(exception, exception.Message);
                return new Result<CartHeaderDto>
                {
                    ErrorMessage = ErrorMessages.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}