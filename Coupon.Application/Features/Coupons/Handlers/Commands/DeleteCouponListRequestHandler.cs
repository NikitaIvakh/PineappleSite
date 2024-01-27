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
    public class DeleteCouponListRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, DeleteListValidator validationRules) : IRequestHandler<DeleteCouponListRequest, Result<List<CouponDto>>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly DeleteListValidator _validator = validationRules;

        public async Task<Result<List<CouponDto>>> Handle(DeleteCouponListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var coupons = await _repository.GetAllAsync().Where(key => request.DeleteCoupon.CouponIds.Contains(key.CouponId)).ToListAsync(cancellationToken);
                var result = await _validator.ValidateAsync(request.DeleteCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    return new Result<List<CouponDto>>
                    {
                        ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                        ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    await _repository.DeleteListAsync(coupons);
                    return new Result<List<CouponDto>>
                    {
                        SuccessMessage = "Купоны успешно удалены",
                        Data = _mapper.Map<List<CouponDto>>(coupons),
                    };
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new Result<List<CouponDto>>
                {
                    ErrorMessage = ErrorMessage.CouponNotDeletedListCatch,
                    ErrorCode = (int)ErrorCodes.CouponNotDeleted,
                };
            }
        }
    }
}