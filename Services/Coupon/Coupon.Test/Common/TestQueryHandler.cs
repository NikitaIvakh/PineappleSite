using Coupon.Infrastructure;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        private readonly ApplicationDbContext _context;
        protected readonly ICouponRepository Repository;
        protected readonly IMemoryCache MemoryCache;

        protected TestQueryHandler()
        {
            _context = CouponRepositoryContextFactory.Create();
            Repository = new CouponRepository(_context);
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Dispose() => CouponRepositoryContextFactory.DestroyDatabase(_context);
    }
}