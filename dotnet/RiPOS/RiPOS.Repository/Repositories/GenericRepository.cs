using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using RiPOS.Database;
using RiPOS.Domain.Interfaces;
using RiPOS.Domain.Shared;
using RiPOS.Repository.Interfaces;
using System.Linq.Expressions;

namespace RiPOS.Repository.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
           where TEntity : TrackEntityChanges, IEntity
    {
        private readonly RiPosDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected GenericRepository(RiPosDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _dbSet.AsNoTracking().AnyAsync(filter);
        }

        public async Task<ICollection<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProps = null, Expression<Func<TEntity, object>> orderBy = null, bool orderDesc = false, int pageNumber = 0, int pageSize = 0)
        {
            var query = GenerateQuery(filter, includeProps, orderBy, orderDesc, pageNumber, pageSize);

            return await query.ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            var query = GenerateQuery(filter);

            return await query.CountAsync();
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _dbSet.AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProps = null)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (includeProps != null)
            {
                query = includeProps(query);
            }

            return await query.SingleOrDefaultAsync(filter);
        }

        public async Task<bool> AddAsync(TEntity entity)
        {
            var currentDate = DateTime.Now;
            entity.CreationDate = currentDate;
            entity.LastModificationDate = currentDate;
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity, params Expression<Func<TEntity, object>>[] propsToIgnore)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            entity.LastModificationDate = DateTime.Now;
            _context.Entry(entity).Property(e => e.CreationDate).IsModified = false;
            _context.Entry(entity).Property(e => e.CreationByUserId).IsModified = false;

            foreach (var prop in propsToIgnore)
            {
                _context.Entry(entity).Property(prop).IsModified = false;
            }

            return await _context.SaveChangesAsync() > 0;
        }

        private IQueryable<TEntity> GenerateQuery(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> includeProps = null, Expression<Func<TEntity, object>> orderBy = null, bool orderDesc = false, int pageNumber = 0, int pageSize = 0)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();


            if (includeProps != null)
            {
                query = includeProps(query);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderDesc ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
            }

            if (pageSize > 0 && pageNumber > 0)
            {
                query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            }

            return query;
        }
    }
}
