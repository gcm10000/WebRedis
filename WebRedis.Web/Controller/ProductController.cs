using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebRedis.Domain.Common.DTO;
using WebRedis.Domain.Common.Entities;
using WebRedis.Infrastructure.Redis.Interfaces;

namespace WebRedis.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ICacheService<Product> _cacheService;

        public ProductController(ICacheService<Product> cacheService)
        {
            this._cacheService = cacheService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var product = await _cacheService.TryGetValueAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductDTO productDTO)
        {
            Product product = new Product()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price,
            };

            var returnedProduct = await _cacheService.TrySetValueAsync(product);
            return Ok(returnedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] Guid id, UpdateProductDTO productDTO)
        {
            var entity = await _cacheService.TryGetValueAsync(id);
            if (entity != default) //Update
            {
                entity.Name = productDTO.Name ?? entity.Name;
                entity.Description = productDTO.Description ?? entity.Description;
                entity.Price = productDTO.Price ?? entity.Price;

                entity.CreatedAt = entity.CreatedAt;
                entity.UpdatedAt = DateTime.Now;
            }

            var returnedProduct = await _cacheService.TrySetValueAsync(entity);
            return returnedProduct != default ? Ok(returnedProduct) : NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var result = await _cacheService.TryRemoveAsync(guid);
            return result ? Ok(result) : NoContent();
        }

    }
}
