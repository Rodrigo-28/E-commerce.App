using E_commerce.application.Dtos.Requests;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this._categoryService = categoryService;
        }
        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetOne(int genreId)
        {
            var category = await _categoryService.GetOne(genreId);

            return Ok(category);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _categoryService.GetAll();

            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto createCategoryDto, IValidator<CreateCategoryDto> validator)
        {
            var vr = await validator.ValidateAsync(createCategoryDto);
            if (!vr.IsValid)
                throw new BadRequestException(vr.ToString()) { ErrorCode = "004" };

            var genre = await _categoryService.Crete(createCategoryDto);

            return CreatedAtAction(
                     actionName: nameof(GetOne),
                     routeValues: new { genreId = genre.Id },
                     value: genre
                 );

        }
    }
}
