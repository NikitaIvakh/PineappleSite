using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Infrastructure.Repository;

public sealed class CouponRepository(ApplicationDbContext context) : ICouponRepository
{
    public IQueryable<CouponEntity> GetAllAsync()
    {
        return context.Coupons.AsNoTracking().AsQueryable();
    }

    public Task<CouponEntity> CreateAsync(CouponEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Сущность пустая");
        }

        context.Add(entity);
        context.SaveChanges();

        return Task.FromResult(entity);
    }

    public Task<CouponEntity> UpdateAsync(CouponEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Сущность пустая");
        }

        context.Update(entity);
        context.SaveChanges();

        return Task.FromResult(entity);
    }

    public Task<CouponEntity> DeleteAsync(CouponEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Сущность пустая");
        }

        context.Remove(entity);
        context.SaveChanges();

        return Task.FromResult(entity);
    }
}