using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;

namespace WebRedis.Domain.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Product> ProductRepository { get; }
        Task BeginTransactionAsync();
        Task<bool> Commit();
    }
}
