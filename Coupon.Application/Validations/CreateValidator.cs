using Coupon.Domain.DTOs;
using FluentValidation;

namespace Coupon.Application.Validations
{
    public class CreateValidator : AbstractValidator<CreateCouponDto>
    {
        public CreateValidator()
        {
            Include(new ICouponRulesValidator());
        }
    }
}