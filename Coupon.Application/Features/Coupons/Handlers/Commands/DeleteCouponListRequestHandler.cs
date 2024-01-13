using Coupon.Application.DTOs.Validator;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Interfaces;
using Coupon.Application.Response;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class DeleteCouponListRequestHandler(ICouponDbContext repository, DeleteCouponListDtoValidator validator) : IRequestHandler<DeleteCouponListRequest, BaseCommandResponse>
    {
        private readonly ICouponDbContext _repository = repository;
        private readonly DeleteCouponListDtoValidator _validator = validator;

        public async Task<BaseCommandResponse> Handle(DeleteCouponListRequest request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validationResult = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.IsSuccess = false;
                    response.Message = "Ошибка удаления";
                    response.ValidationErrors = validationResult.Errors.Select(key => key.ErrorMessage).ToList();
                }

                else
                {
                    var coupons = await _repository.Coupons.Where(key => request.DeleteCoupon.CouponIds.Contains(key.CouponId)).ToListAsync(cancellationToken);

                    _repository.Coupons.RemoveRange(coupons);
                    await _repository.SaveChangesAsync(cancellationToken);

                    response.IsSuccess = true;
                    response.Message = "Купоны успешно удалены";

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