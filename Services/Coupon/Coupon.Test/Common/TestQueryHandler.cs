using AutoMapper;
using Coupon.Application.Mapping;
using Coupon.Infrastructure;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Test.Common;

public class TestQueryHandler : IDisposable
{
    private readonly ApplicationDbContext _context;
    protected readonly ICouponRepository Repository;
    protected readonly IMemoryCache MemoryCache;
    protected readonly IMapper Mapper;

    protected TestQueryHandler()
    {
        _context = CouponRepositoryContextFactory.Create();
        Repository = new CouponRepository(_context);
        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        var mapperConfiguration = new MapperConfiguration(options => { options.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
    }

    public void Dispose() => CouponRepositoryContextFactory.DestroyDatabase(_context);
}