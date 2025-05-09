﻿using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using RiPOS.Domain.Shared;

namespace RiPOS.Repository.Interfaces;

public interface IGenericRepository<TEntity> where TEntity : TrackEntityChanges
{
    Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);

    Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProps = null, Expression<Func<TEntity, object>>? orderBy = null, bool orderDesc = false, int pageNumber = 0, int pageSize = 0);

    Task<int> CountAsync(Expression<Func<TEntity, bool>> filter);

    Task<TEntity?> GetByIdAsync(int id);

    Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? includeProps = null);

    Task AddAsync(TEntity entity);

    void Update(TEntity entity, params Expression<Func<TEntity, object>>[] propsToIgnore);
}