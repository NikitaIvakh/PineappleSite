using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces.Repository;

namespace Order.Infrastructure.Repository.Implementation
{
    public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>().AsNoTracking();
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity is not null)
            {
                _context.Add(entity);
                _context.SaveChanges();

                return Task.FromResult(entity);
            }

            throw new ArgumentNullException(nameof(entity), "Объект пустой");
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task DeleteAsync(TEntity entity)
        {
            if (entity is not null)
            {
                _context.Remove(entity);
                _context.SaveChanges();

                return Task.FromResult(entity);
            }

            throw new ArgumentNullException(nameof(entity), "Объект пустой");
        }
    }
}