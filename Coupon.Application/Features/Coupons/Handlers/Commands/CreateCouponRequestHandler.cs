using AutoMapper;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources;
using Coupon.Application.Validations;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class CreateCouponRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, CreateValidator createValidator) : IRequestHandler<CreateCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<CreateCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly CreateValidator _createValidator = createValidator;

        public async Task<Result<CouponDto>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponCode == request.CreateCoupon.CouponCode, cancellationToken);
                var result = await _createValidator.ValidateAsync(request.CreateCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotCreated,
                        ErrorCode = (int)ErrorCodes.CouponNotCreated,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    coupon = new CouponEntity
                    {
                        CouponCode = request.CreateCoupon.CouponCode,
                        DiscountAmount = request.CreateCoupon.DiscountAmount,
                        MinAmount = request.CreateCoupon.MinAmount,
                    };

                    await _repository.CreateAsync(coupon);

                    return new Result<CouponDto>
                    {
                        Data = _mapper.Map<CouponDto>(coupon),
                        SuccessMessage = "Купон успешно создан",
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}