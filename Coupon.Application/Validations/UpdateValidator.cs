using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class UpdateValidator : AbstractValidator<UpdateCouponDto>
    {
        public UpdateValidator()
        {
            Include(new ICouponRulesValidator());
        }
    }
}