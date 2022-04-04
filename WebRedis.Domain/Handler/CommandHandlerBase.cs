using Microsoft.Extensions.Configuration;
using WebRedis.Domain.Common.Interfaces;

namespace WebRedis.Domain.Handler
{
    public abstract class CommandHandlerBase<TEntity>
    {
        protected readonly IConfiguration _configuration;

        public CommandHandlerBase(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
    }
}
