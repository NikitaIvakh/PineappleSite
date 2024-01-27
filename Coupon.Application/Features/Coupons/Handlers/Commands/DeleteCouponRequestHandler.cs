using Coupon.Application.DTOs.Validator;
using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using Coupon.Application.Response;
using Coupon.Domain.Entities;
using MediatR;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class DeleteCouponRequestHandler(ICouponDbContext couponDbContext, DeleteCouponDtoValidator validator) : IRequestHandler<DeleteCouponRequest, BaseCommandResponse>
    {
        private readonly ICouponDbContext _repository = couponDbContext;
        private readonly DeleteCouponDtoValidator _validator = validator;

        public async Task<BaseCommandResponse> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validationResult = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка удаления купона";
                    response.ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var coupon = await _repository.Coupons.FindAsync(new object[] { request.DeleteCoupon.Id }, cancellationToken) ??
                        throw new NotFoundException(nameof(CouponEntity), request.DeleteCoupon.Id);

                    _repository.Coupons.Remove(coupon);
                    await _repository.SaveChangesAsync(cancellationToken);

                    response.IsSuccess = true;
                    response.Message = "Купон успешно удален";
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