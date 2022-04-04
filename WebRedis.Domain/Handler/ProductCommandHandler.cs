using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Threading;
using System.Threading.Tasks;
using WebRedis.Domain.Commands.Product;
using WebRedis.Domain.Common.Entities;
using WebRedis.Domain.Services.Interfaces;

namespace WebRedis.Domain.Handler
{
    public class ProductCommandHandler : CommandHandlerBase<Product>,
        IRequestHandler<ProductCommandCreate, ValidationResult>,
        IRequestHandler<ProductCommandUpdate, ValidationResult>,
        IRequestHandler<ProductCommandDelete, ValidationResult>
    {
        private readonly IProductService _productService;

        public ProductCommandHandler(IProductService service, IConfiguration configuration) : base(configuration)
        {
            this._productService = service;
        }

        public async Task<ValidationResult> Handle(ProductCommandCreate request, CancellationToken cancellationToken)
        {
            if (request.IsValid())
            {
                Product product = new()
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                };

                //var returnedProduct = await _cacheService.TrySetValueAsync(product);
                await _productService.CreateAsync(product);
            }

            return request.ValidationResult;
        }

        public async Task<ValidationResult> Handle(ProductCommandUpdate request, CancellationToken cancellationToken)
        {
            if (request.IsValid())
            {

                //var entity = await _cacheService.TryGetValueAsync(request.Id);
                //if (entity != default)
                //{
                //    entity.Name = request.Name ?? entity.Name;
                //    entity.Description = request.Description ?? entity.Description;
                //    entity.Price = request.Price ?? entity.Price;

                //    entity.CreatedAt = entity.CreatedAt;
                //    entity.UpdatedAt = DateTime.Now;
                //}

                Product product = new()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price.Value,
                };

                await _productService.UpdateAsync(product);


            }

            return request.ValidationResult;
        }

        public async Task<ValidationResult> Handle(ProductCommandDelete request, CancellationToken cancellationToken)
        {
            if (request.IsValid())
            {
                await _productService.DeleteAsync(request.Id);
            }

            return request.ValidationResult;
        }
    }
}
