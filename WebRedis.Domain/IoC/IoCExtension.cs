using MediatR;
using Microsoft.Extensions.DependencyInjection;
using WebRedis.Domain.Handler;
using WebRedis.Domain.Services;
using WebRedis.Domain.Services.Interfaces;

namespace WebRedis.Domain.IoC
{
    public static class IoCExtension
    {
        public static void ConfigureServicesDomainApi(this IServiceCollection services)
        {
            services.AddMediatR(typeof(ProductCommandHandler));

            services.AddScoped<IProductService, ProductService>();
        }
    }
}
