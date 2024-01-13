using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Interfaces;
using Coupon.Core.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponDetailsRequestHandler(ICouponDbContext repository, IMapper mapper) : IRequestHandler<GetCouponDetailsRequest, CouponDto>
    {
        private readonly ICouponDbContext _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<CouponDto> Handle(GetCouponDetailsRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _repository.Coupons.FirstOrDefaultAsync(key => key.CouponId == request.Id, cancellationToken) ??
                throw new NotFoundException(nameof(CouponEntity), request.Id);

            return _mapper.Map<CouponDto>(coupon);
        }
    }
}