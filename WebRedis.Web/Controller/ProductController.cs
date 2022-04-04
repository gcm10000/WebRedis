using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WebRedis.Domain.Commands.Product;
using WebRedis.Domain.Common.DTO;
using WebRedis.Domain.Services.Interfaces;

namespace WebRedis.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        protected readonly IMediator _mediator;

        public ProductController(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync(string filter)
        {
            var products = await _productService.GetAllAsync(filter);
            return Ok(products);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var product = await _productService.GetAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateProductDTO productDTO)
        {
            var command = new ProductCommandCreate()
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price
            };

            var returnedProduct = await _mediator.Send(command);

            return Ok(returnedProduct);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromQuery] Guid id, UpdateProductDTO productDTO)
        {
            var command = new ProductCommandUpdate()
            {
                Id = id,
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price.Value
            };

            var returnedProduct = await _mediator.Send(command);

            return returnedProduct != default ? Ok(returnedProduct) : NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid guid)
        {
            var response = await _mediator.Send(new ProductCommandDelete() { Id = guid });

            return response != default ? Ok(response) : NoContent();
        }

    }
}
