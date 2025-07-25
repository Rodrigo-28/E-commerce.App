using E_commerce.application.Dtos.Requests;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            this._productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductQueryDto queryDto, IValidator<ProductQueryDto> validator)
        {
            var validationResult = await validator.ValidateAsync(queryDto);
            if (!validationResult.IsValid)
            {
                // Aquí puedes lanzar tu excepción centralizada
                throw new BadRequestException(validationResult.ToString()) { ErrorCode = "004" };
            }
            var items = await _productService.GetAll(queryDto);
            return Ok(items);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _productService.GetById(id);
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto, IValidator<CreateProductDto> validator)
        {
            var validationResult = await validator.ValidateAsync(createProductDto);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString())
                {
                    ErrorCode = "004"
                };
            }

            var created = await _productService.CreateAsync(createProductDto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateProductDto createProductDto, IValidator<CreateProductDto> validator)
        {
            var validationResult = await validator.ValidateAsync(createProductDto);
            if (!validationResult.IsValid)
            {
                throw new BadRequestException(validationResult.ToString())
                {
                    ErrorCode = "004"
                };
            }
            var updated = await _productService.Update(id, createProductDto);
            return Ok(updated);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var productDelete = await _productService.Delete(id);

            return Ok(productDelete);
        }


    }
}
