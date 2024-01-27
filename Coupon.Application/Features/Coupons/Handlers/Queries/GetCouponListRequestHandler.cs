using AutoMapper;
using AutoMapper.QueryableExtensions;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Interfaces;
using Coupon.Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Application.Features.Coupons.Handlers.Queries
{
    public class GetCouponListRequestHandler(ICouponDbContext repository, IMapper mapper) : IRequestHandler<GetCouponListRequest, IEnumerable<CouponDto>>
    {
        private readonly ICouponDbContext _repository = repository;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CouponDto>> Handle(GetCouponListRequest request, CancellationToken cancellationToken)
        {
            var coupons = await _repository.Coupons.ProjectTo<CouponDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
    }
}