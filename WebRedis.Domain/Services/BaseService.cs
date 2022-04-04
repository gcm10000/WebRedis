using Microsoft.Extensions.Configuration;

namespace WebRedis.Domain.Services
{
    public abstract class BaseService<TEntity>
    {
        protected bool UseCaching { get; }

        public BaseService(IConfiguration configuration)
        {
            UseCaching = bool.Parse(configuration["UseCaching"]);
        }

        protected string BuildCacheKey(string key)
        {
            return $"{typeof(TEntity).FullName}:{key}";
        }
    }
}