using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Infrastructure.Redis.Extensions;

namespace WebRedis.Infrastructure.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisCacheService> _log;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer, ILogger<RedisCacheService> log)
        {
            _db = connectionMultiplexer.GetDatabase();
            this._log = log;
        }

        #region Private methods
        private string BuildCacheKey<T>(string key, bool persist = true)
        {
            string keyCloakUserId = "Unauthenticated";
            //var _principal = ClaimsPrincipal.Current;

            //if (_principal != null && _principal.Identity.IsAuthenticated)
            //    keyCloakUserId = _principal.FindFirst(USER_ID_CLAIM_TYPE).Value;

            string newKey = string.Format("{0}:{1}", keyCloakUserId, key);

            if (persist)
            {
                string controlKey = $"{typeof(T).GetFriendlyName()}";
                List<string> cachedKeys = TryGetValue<List<string>>(controlKey);

                if (cachedKeys == null)
                    cachedKeys = new List<string>();

                if (!cachedKeys.Contains(newKey))
                {
                    cachedKeys.Add(newKey);
                    TrySetValue(controlKey, cachedKeys, new DistributedCacheEntryOptions());
                }
            }

            return newKey;
        }
        private T TryGetValue<T>(string key)
        {
            T value = default(T);

            try
            {
                var cachedString = _db.StringGet(key);

                if (!string.IsNullOrEmpty(cachedString))
                    value = JsonConvert.DeserializeObject<T>(cachedString);
            }
            catch (Exception ex)
            {
                _log.LogError("Failed to get data from Redis Server", ex);
            }

            return value;

        }
        private async Task<T> TryGetValueAsync<T>(string key)
        {
            T value = default(T);

            try
            {
                var cachedString = await _db.StringGetAsync(key);

                if (!string.IsNullOrEmpty(cachedString))
                    value = JsonConvert.DeserializeObject<T>(cachedString);
            }
            catch (Exception ex)
            {
                _log.LogError("Failed to get data from Redis Server", ex);
            }

            return value;

        }
        private void TrySetValue<T>(string key, T value, DistributedCacheEntryOptions options)
        {
            try
            {
                _db.StringSet(key,
                    JsonConvert.SerializeObject(value,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }),

                    TimeSpan.FromMinutes(5));
            }
            catch (Exception ex)
            {
                _log.LogError("Failed to write data into Redis Server", ex);
            }
        }
        private void TryRemove(string key)
        {
            try
            {
                _db.KeyDelete(key);
            }
            catch (Exception ex)
            {
                _log.LogError("Failed to remove key from Redis Server", ex);
            }
        }
        private async Task TryRemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _log.LogError("Failed to remove key from Redis Server", ex);
            }
        }
        #endregion

        public virtual T GetEntity<TEntity, T>(string key)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);

            return TryGetValue<T>(cacheKey);
        }

        public virtual T SetEntity<TEntity, T>(T entity, string key, DistributedCacheEntryOptions options)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);
            TrySetValue<T>(cacheKey, entity, options);

            return entity;
        }

        public virtual void RemoveEntity<T>(string key)
        {
            TryRemove(BuildCacheKey<T>(key, false));
        }

        public virtual async Task<T> GetEntityAsync<TEntity, T>(string key)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);

            return await TryGetValueAsync<T>(cacheKey);
        }

        public virtual async Task<T> SetEntityAsync<TEntity, T>(T entity, string key, DistributedCacheEntryOptions options)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);
            TrySetValue<T>(cacheKey, entity, options);

            return await Task.FromResult(entity);
        }

        public virtual async Task RemoveEntityAsync<T>(string key)
        {
            await TryRemoveAsync(BuildCacheKey<T>(key, false));
        }

        public T GetEntity<Tentity, T>(string key, Func<T> newDataCallback)
        {
            string cacheKey = BuildCacheKey<Tentity>(key);

            T value = TryGetValue<T>(cacheKey);

            if (value == null || value.Equals(default(T)))
            {
                value = newDataCallback();
                TrySetValue<T>(cacheKey, value, new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromMinutes(5)
                });
            }

            return value;
        }

        public async Task<T> GetEntityAsync<TEntity, T>(string key, Func<T> newDataCallback, DistributedCacheEntryOptions options = null)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);

            T value = await TryGetValueAsync<T>(cacheKey);

            if (value == null || value.Equals(default(T)))
            {
                value = newDataCallback();
                TrySetValue(BuildCacheKey<T>(key), value, options);
            }

            return value;
        }
        public T GetEntity<TEntity, T>(string key, Func<T> newDataCallback, DistributedCacheEntryOptions options = null)
        {
            string cacheKey = BuildCacheKey<TEntity>(key);

            T value = TryGetValue<T>(cacheKey);

            if (value == null || value.Equals(default(T)))
            {
                value = newDataCallback();
                TrySetValue(cacheKey, value, options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(5) });
            }

            return value;
        }

        public async Task InvalidateCacheAsync<T>()
        {
            List<string> cachedKeys = await TryGetValueAsync<List<string>>($"{typeof(T).GetFriendlyName()}");

            if (cachedKeys != null)
            {
                foreach (string key in cachedKeys)
                    await TryRemoveAsync(key);

                TrySetValue<List<string>>($"{typeof(T).GetFriendlyName()}", new List<string>(), new DistributedCacheEntryOptions());
            }
        }

        public void InvalidateCache<T>()
        {
            List<string> cachedKeys = TryGetValue<List<string>>($"{typeof(T).GetFriendlyName()}");

            if (cachedKeys != null)
            {
                foreach (string key in cachedKeys)
                    TryRemove(key);

                TrySetValue<List<string>>($"{typeof(T).GetFriendlyName()}", new List<string>(), new DistributedCacheEntryOptions());
            }
        }
    }
}
