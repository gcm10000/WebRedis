using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;

namespace WebRedis.Domain.Services.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> GetProductAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<bool> DeleteProductAsync(Guid id);

    }
}
