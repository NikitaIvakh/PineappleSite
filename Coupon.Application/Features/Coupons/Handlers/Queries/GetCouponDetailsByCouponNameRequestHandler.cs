using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Application.Exceptions;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Interfaces;
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
            var coupon = await _context.Coupons.FirstOrDefaultAsync(key => key.CouponCode == request.CouponCode, cancellationToken) ??
                throw new NotFoundException($"Купон с названием: ", request.CouponCode);

            return _mapper.Map<CouponDto>(coupon);
        }
    }
}