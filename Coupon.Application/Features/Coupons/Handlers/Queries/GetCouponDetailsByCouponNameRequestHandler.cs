using AutoMapper;
using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Interfaces;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponDetailsByCouponNameRequestHandler(ICouponDbContext context, IMapper mapper) : IRequestHandler<GetCouponDetailsByCouponNameRequest, CouponDto>
    {
        private readonly ICouponDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<CouponDto> Handle(GetCouponDetailsByCouponNameRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(key => key.CouponCode.ToLower() == request.CouponCode.ToLower(), cancellationToken) ??
                throw new NotFoundException(nameof(CouponEntity), request.CouponCode);

            return _mapper.Map<CouponDto>(coupon);
        }
    }
}