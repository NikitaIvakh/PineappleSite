using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces.Repository;

namespace Order.Infrastructure.Repository.Implementation;

public sealed class BaseRepository<TEntity>(ApplicationDbContext context)
    : IBaseRepository<TEntity> where TEntity : class
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

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        if (entity is null)
        {
            throw new ArgumentNullException(nameof(entity), "Оюъект пустой");
        }

        context.Update(entity);
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