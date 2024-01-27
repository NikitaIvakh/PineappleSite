using Coupon.Domain.Interfaces.Repositories;

namespace Coupon.Infrastructure.Repository
{
    public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<TEntity> GetAllAsync()
        {
            return _context.Set<TEntity>();
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Сущность пустая");
            }

            _context.Add(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Сущность пустая");
            }

            _context.Update(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException("Сущность пустая");
            }

            _context.Remove(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }
    }
}