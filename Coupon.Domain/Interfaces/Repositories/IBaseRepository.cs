namespace Coupon.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        Task<IQueryable<TEntity>> GetAllAsync();

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task<TEntity> DeleteAsync(TEntity entity);
    }
}