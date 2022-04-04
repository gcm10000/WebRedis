using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;

namespace WebRedis.Domain.Common.Interfaces
{
    public interface IRepository<TEntity>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> query);
        Task<TEntity> GetAsync(Guid Id);
        Task<TEntity> CreateAsync(TEntity product);
        Task<TEntity> UpdateAsync(TEntity product);
        Task DeleteAsync(Guid id);
    }
}
