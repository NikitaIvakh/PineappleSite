using Favourite.Domain.Interfaces.Repository;
using Microsoft.EntityFrameworkCore;

namespace Favourite.Infrastructure.Repository.Implement;

public sealed class BaseRepository<TEntity>(ApplicationDbContext context) : IBaseRepository<TEntity> where TEntity : class
{
    public IQueryable<TEntity> GetAll()
    {
        return context.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Объект пустой");
        }

        await context.AddAsync(entity);
        await context.SaveChangesAsync();

        return await Task.FromResult(entity);
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Объект пустой");
        }

        context.Remove(entity);
        await context.SaveChangesAsync();

        return await Task.FromResult(entity);
    }
}