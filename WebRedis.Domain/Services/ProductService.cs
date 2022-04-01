using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Services.Interfaces;
using WebRedis.Infrastructure.Redis.Interfaces;

namespace WebRedis.Domain.Services
{
    public class ProductService : IProductService
    {
        //private readonly ICacheService<Product> _cacheService;
        //public ProductService(ICacheService<Product> cacheService)
        //{
        //    _cacheService = cacheService;
        //}
        //public Task<Product> CreateProductAsync(Product product)
        //{

        //}

        //public Task<bool> DeleteProductAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<IEnumerable<Product>> GetAllProductsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Product> GetProductAsync(Guid id)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Product> UpdateProductAsync(Product product)
        //{
        //    throw new NotImplementedException();
        //}
        public Task<Product> CreateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
