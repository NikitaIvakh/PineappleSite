using Coupon.Application.Validations;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure;
using Coupon.Infrastructure.Repository;
using Microsoft.Extensions.Caching.Memory;

namespace Coupon.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected readonly ApplicationDbContext Context;
    protected readonly CreateValidator CreateValidator;
    protected readonly UpdateValidator UpdateValidator;
    protected readonly DeleteValidator DeleteValidator;
    protected readonly DeleteCouponsValidator DeleteCouponsValidator;
    protected readonly ICouponRepository Repository;
    protected readonly IMemoryCache MemoryCache;

    protected TestCommandHandler()
    {
        Context = CouponRepositoryContextFactory.Create();
        Repository = new CouponRepository(Context);
        CreateValidator = new CreateValidator();
        UpdateValidator = new UpdateValidator();
        DeleteValidator = new DeleteValidator();
        DeleteCouponsValidator = new DeleteCouponsValidator();

        MemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public void Dispose() => CouponRepositoryContextFactory.DestroyDatabase(Context);
}