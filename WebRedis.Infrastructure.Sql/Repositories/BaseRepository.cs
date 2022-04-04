using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Infrastructure.Sql.DbContexts;

namespace WebRedis.Infrastructure.Sql
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> Entity;
        private readonly SqlDbContext _dbContext;

        public BaseRepository(SqlDbContext context)
        {
            this.Entity = context.Set<TEntity>();
            this._dbContext = context;
        }

        public async Task<TEntity> CreateAsync(TEntity product)
        {
            await Entity.AddAsync(product);
            
            return product;
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            Entity.Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = Entity.AsNoTracking();
            
            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetAsync(Guid Id)
        {
            return await Entity.FindAsync(new object[] { Id });
        }

        public async Task<TEntity> UpdateAsync(TEntity product)
        {
            Entity.Attach(product);
            _dbContext.Entry(product).State = EntityState.Modified;
         
            return product;
        }
    }
}