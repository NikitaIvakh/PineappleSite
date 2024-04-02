using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Coupon.Infrastructure.Repository
{
    public class CouponRepository(ApplicationDbContext context) : ICouponRepository
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<CouponEntity> GetAllAsync()
        {
            return _context.Coupons.AsNoTracking().AsQueryable();
        }

        public Task<CouponEntity> CreateAsync(CouponEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Сущность пустая");
            }

            _context.Add(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<CouponEntity> UpdateAsync(CouponEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Сущность пустая");
            }

            _context.Update(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<CouponEntity> DeleteAsync(CouponEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Сущность пустая");
            }

            _context.Remove(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }
    }
}