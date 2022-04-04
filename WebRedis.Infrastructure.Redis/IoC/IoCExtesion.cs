using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using WebRedis.Domain.Common.Interfaces;

namespace WebRedis.Infrastructure.Redis.IoC
{
    public static class IoCExtesion
    {
        public static void ConfigureServiceRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(x => ConnectionMultiplexer.Connect(configuration["RedisConnection"]));
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}
