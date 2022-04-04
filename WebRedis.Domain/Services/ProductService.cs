using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Common.Interfaces;
using WebRedis.Domain.Services.Interfaces;

namespace WebRedis.Domain.Services
{
    public sealed class ProductService : BaseService<Product>, IProductService
    {
        private readonly ICacheService _cacheService;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(ICacheService cacheService, IUnitOfWork unitOfWork, IConfiguration configuration) : base(configuration)
        {
            this._cacheService = cacheService;
            this._unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Product>> GetAllAsync(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
                filter = "";

            Expression<Func<Product, bool>> query = p => p.Name.Contains(filter) || p.Description.Contains(filter);
            IEnumerable<Product> data;

            if (UseCaching)
                data = await _cacheService.GetEntityAsync<IEnumerable<Product>, IEnumerable<Product>>(BuildCacheKey($"GetAllFiltered:{filter}"), () => _unitOfWork.ProductRepository.GetAllAsync(query).Result);
            else
                data = await _unitOfWork.ProductRepository.GetAllAsync(query);

            return data;
        }

        public async Task<Product> GetAsync(Guid id)
        {
            Product data;

            if (UseCaching)
                data = await _cacheService.GetEntityAsync<Product, Product>(BuildCacheKey($"FindById:{id}"), () => _unitOfWork.ProductRepository.GetAsync(id).Result);
            else
                data = await _unitOfWork.ProductRepository.GetAsync(id);

            return data;
        }
        //c2a22e19-caef-444c-9e0d-1e0f0e139c8b
        public async Task CreateAsync(Product product)
        {
            await _unitOfWork.BeginTransactionAsync();

            product.CreatedAt = DateTime.Now;
            product.UpdatedAt = DateTime.Now;

            await _unitOfWork.ProductRepository.CreateAsync(product);

            await _unitOfWork.Commit();

            await _cacheService.InvalidateCacheAsync<Product>();
        }

        public async Task UpdateAsync(Product product)
        {
            await _unitOfWork.BeginTransactionAsync();

            product.UpdatedAt = DateTime.Now;

            await _unitOfWork.ProductRepository.UpdateAsync(product);
        
            await _unitOfWork.Commit();

            await _cacheService.InvalidateCacheAsync<Product>();
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.BeginTransactionAsync();

            await _unitOfWork.ProductRepository.DeleteAsync(id);
        
            await _unitOfWork.Commit();
            
            await _cacheService.InvalidateCacheAsync<Product>();
        }
    }
}
