using AutoMapper.QueryableExtensions;
using Coupon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PineappleSite.Coupon.Core.Interfaces;

namespace Coupon.Infrastructure.Repository
{
    public class CouponRepository(ApplicationDbContext context) : ICouponRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<CouponEntity>> GetAllAsync()
        {
            return await _context.Coupons.OrderBy(key => key.CouponCode).ToListAsync();
        }

        public async Task<CouponEntity> GetAsync(int couponId)
        {
            return await _context.Coupons.FirstAsync(key => key.CouponId == couponId);
        }

        public async Task CreateAsync(CouponEntity coupon)
        {
            await _context.Coupons.AddAsync(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CouponEntity coupon)
        {
            _context.Entry(coupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int couponId)
        {
            var coupon = await _context.Coupons.FirstAsync(key => key.CouponId == couponId);
            if (coupon != null)
            {
                _context.Coupons.Remove(coupon);
                await _context.SaveChangesAsync();
            }
        }
    }
}