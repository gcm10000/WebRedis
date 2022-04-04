using Microsoft.EntityFrameworkCore;
using WebRedis.Domain.Common.Entities;

namespace WebRedis.Infrastructure.Sql.DbContexts
{
    public class SqlDbContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public SqlDbContext(DbContextOptions<SqlDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }
    }
}
