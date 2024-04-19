﻿namespace Favourite.Domain.Interfaces.Repository;

public interface IBaseRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll();

    Task<TEntity> CreateAsync(TEntity entity);

    Task<TEntity> DeleteAsync(TEntity entity);
}