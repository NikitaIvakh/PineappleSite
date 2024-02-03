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
    public class DeleteCouponRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, DeleteValidator validator) : IRequestHandler<DeleteCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<DeleteCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly DeleteValidator _validator = validator;

        public async Task<Result<CouponDto>> Handle(DeleteCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeleted,
                        ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponId == request.DeleteCoupon.Id, cancellationToken);

                    if (coupon is null)
                    {
                        return new Result<CouponDto>
                        {
                            ErrorMessage = ErrorMessage.CouponNotFound,
                            ErrorCode = (int)ErrorCodes.CouponNotFound,
                            Data = null,
                        };
                    }

                    else
                    {
                        await _repository.DeleteAsync(coupon);
                        return new Result<CouponDto>
                        {
                            Data = _mapper.Map<CouponDto>(coupon),
                            SuccessMessage = "Купон успешно удален",
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.CouponNotDeletedCatch,
                    ErrorCode = (int)ErrorCodes.CouponNotDeletedCatch,
                };
            }
        }
    }
}