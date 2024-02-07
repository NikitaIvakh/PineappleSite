namespace Favourite.Domain.Interfaces.Repository
{
    public interface IBaseRepositiry<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        Task<TEntity> CreateAsync(TEntity entity);

        Task<TEntity> UpdateAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}