using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Infrastructure.Sql.DbContexts;

namespace WebRedis.Infrastructure.Sql.IoC
{
    public static class IocExtension
    {
        public static void ConfigureServiceSql(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRepository<Product>, ProductRepository>();

            services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
