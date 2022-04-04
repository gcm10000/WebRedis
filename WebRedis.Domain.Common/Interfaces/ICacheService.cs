using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace WebRedis.Domain.Common.Interfaces
{
    public interface ICacheService
    {
        T GetEntity<TEntity, T>(string key);

        T GetEntity<TEntity, T>(string key, Func<T> newDataCallback);

        T GetEntity<TEntity, T>(string key, Func<T> newDataCallback, DistributedCacheEntryOptions options = null);

        T SetEntity<TEntity, T>(T entity, string key, DistributedCacheEntryOptions options);

        Task<T> GetEntityAsync<TEntity, T>(string key);

        Task<T> GetEntityAsync<TEntity, T>(string key, Func<T> newDataCallback, DistributedCacheEntryOptions options = null);

        Task<T> SetEntityAsync<TEntity, T>(T entity, string key, DistributedCacheEntryOptions options);

        Task RemoveEntityAsync<T>(string key);

        Task InvalidateCacheAsync<T>();

        void RemoveEntity<T>(string key);

        void InvalidateCache<T>();
    }
}
