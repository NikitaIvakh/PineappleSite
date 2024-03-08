using Product.Domain.Interfaces;

namespace Product.Infrastructure.Repository
{
    public class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context = context;

        public IQueryable<TEntity> GetAll()
        {
            return _context.Set<TEntity>();
        }

        public Task<TEntity> CreateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Объект пустой");
            }

            _context.Add(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Объект пустой");
            }

            _context.Update(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);

        }

        public Task<TEntity> DeleteAsync(TEntity entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity), "Объект пустой");
            }

            _context.Remove(entity);
            _context.SaveChanges();

            return Task.FromResult(entity);
        }

        public Task DeleteListAsync(List<TEntity> entities)
        {
            if (entities is null)
            {
                throw new ArgumentNullException(nameof(entities), "Список сущностей пуст.");
            }

            _context.RemoveRange(entities);
            _context.SaveChanges();

            return Task.FromResult(entities);
        }
    }
}