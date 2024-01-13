using AutoMapper;
using Coupon.Application.DTOs.Validator;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using Coupon.Application.Response;
using Coupon.Core.Entities;
using MediatR;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class CreateCouponRequestHandler(ICouponDbContext couponDbContext, IMapper mapper, CreateCouponDtoValidator createCouponValidator) : IRequestHandler<CreateCouponRequest, BaseCommandResponse>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly IMapper _mapper = mapper;
        private readonly CreateCouponDtoValidator _createCouponValidator = createCouponValidator;

        public async Task<BaseCommandResponse> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validationResult = await _createCouponValidator.ValidateAsync(request.CreateCouponDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка при создании купона";
                    response.ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var coupon = _mapper.Map<CouponEntity>(request.CreateCouponDto);

                    await _repository.Coupons.AddAsync(coupon, cancellationToken);
                    await _repository.SaveChangesAsync(cancellationToken);

                    response.IsSuccess = true;
                    response.Message = "Купон успешно создан";
                    response.Id = coupon.CouponId;

                    return response;
                }
            }

            catch (Exception exception)
            {
                response.IsSuccess = false;
                response.Message = exception.Message;
            }

            return response;
        }
    }
}