using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;
using WebRedis.Infrastructure.Redis.Interfaces;

namespace WebRedis.Infrastructure.Redis
{
    public class RedisCacheService<TEntity> : ICacheService<TEntity>
    {
        private readonly IDatabase db;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            db = connectionMultiplexer.GetDatabase();
        }

        public async Task<TEntity> TrySetValueAsync(TEntity value)
        {
            var valueObject = (object)value;
            var valueEntity = (EntityBase)valueObject;

            var id = valueEntity.Id.ToString();
            var key = BuildCacheKey(id);

            await SetCacheValueAsync(key, 
                JsonConvert.SerializeObject(valueEntity,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

            return value;
        }


        public async Task<TEntity> TryGetValueAsync(Guid id)
        {
            var key = BuildCacheKey(id.ToString());
            var value = await GetCacheValueAsync(key);
            return value != null ? JsonConvert.DeserializeObject<TEntity>(value) : default(TEntity);
        }

        public async Task<bool> TryRemoveAsync(Guid id)
        {
            var key = BuildCacheKey(id.ToString());
            return await db.KeyDeleteAsync(key);
        }

        private string BuildCacheKey(string key)
        {
            return $"{typeof(TEntity).FullName}:{key}";
        }

        private async Task<string> GetCacheValueAsync(string key)
        {
            return await db.StringGetAsync(key);
        }

        private async Task<bool> SetCacheValueAsync(string key, string value)
        {
            return await db.StringSetAsync(key, value);
        }
    }
}
