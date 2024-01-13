using AutoMapper;
using Coupon.Application.DTOs.Validator;
using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using Coupon.Application.Response;
using Coupon.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class UpdateCouponRequestHandler(ICouponDbContext couponDbContext, IMapper mapper, UpdateCouponDtoValidator validator) : IRequestHandler<UpdateCouponRequest, BaseCommandResponse>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly IMapper _mapper = mapper;
        private readonly UpdateCouponDtoValidator _validator = validator;

        public async Task<BaseCommandResponse> Handle(UpdateCouponRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validationResult = await _validator.ValidateAsync(request.UpdateCoupon, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка при обновлении купона";
                    response.ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var coupon = await _repository.Coupons.FirstAsync(key => key.CouponId == request.UpdateCoupon.CouponId, cancellationToken: cancellationToken) ??
                        throw new NotFoundException(nameof(CouponEntity), request.UpdateCoupon.CouponId);

                    _mapper.Map(request.UpdateCoupon, coupon);
                    _repository.Coupons.Update(coupon);
                    await _repository.SaveChangesAsync(cancellationToken);

                    response.IsSuccess = true;
                    response.Message = "Купон успешно обновлен";
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