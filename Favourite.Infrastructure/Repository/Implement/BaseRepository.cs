using Favourite.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Favourite.Infrastructure.Repository.Implement
{
    public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepositiry<TEntity> where TEntity : class
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
            if (entity is not null)
            {
                _context.Update(entity);
                _context.SaveChanges();

                return Task.FromResult(entity);
            }

            throw new ArgumentNullException(nameof(entity), "Объект пустой");
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