using System;
using System.Threading.Tasks;

namespace WebRedis.Infrastructure.Redis.Interfaces
{
    public interface ICacheService<TEntity>
    {
        Task<TEntity> TryGetValueAsync(Guid id);
        Task<TEntity> TrySetValueAsync(TEntity value);
        Task<bool> TryRemoveAsync(Guid id);
    }
}
