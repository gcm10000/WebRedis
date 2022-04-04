using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Infrastructure.Sql.DbContexts;

namespace WebRedis.Infrastructure.Sql
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(SqlDbContext context) : base(context)
        {

        }
    }
}