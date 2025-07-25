using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;

namespace E_commerce.application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            this._categoryRepository = categoryRepository;
            this._mapper = mapper;
        }
        public async Task<CategoryResponseDto> Crete(CreateCategoryDto createCategoryDto)
        {
            var categoryExists = await _categoryRepository.GetOne(c => c.Name.ToUpper().Trim() == createCategoryDto.Name.Trim().ToUpper());

            if (categoryExists != null)
            {
                throw new BadRequestException($"category with name '{createCategoryDto.Name}' already exists")
                {
                    ErrorCode = "006"
                };
            }
            var newCategory = new Category
            {
                Name = createCategoryDto.Name,
            };
            var category = await _categoryRepository.create(newCategory);
            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<bool> Delete(int categoryId)
        {
            var category = await _categoryRepository.GetOne(categoryId);
            if (category == null) return false;

            return await _categoryRepository.delete(category);
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAll()
        {
            var category = await _categoryRepository.GetAll();

            if (category == null)
            {
                throw new BadRequestException($"There are no categories in the database")
                {
                    ErrorCode = "005"
                };
            }
            return _mapper.Map<IEnumerable<CategoryResponseDto>>(category);
        }

        public async Task<CategoryResponseDto> GetOne(int categoryId)
        {
            var category = await _categoryRepository.GetOne(categoryId);
            if (category == null)
            {
                throw new NotFoundException("category not found")
                {
                    ErrorCode = "003"
                };
            }
            return _mapper.Map<CategoryResponseDto>(category);
        }

        public async Task<CategoryResponseDto> update(int CategoriesId, CreateCategoryDto createCategoryDto)
        {
            var currentCategory = await _categoryRepository.GetOne(CategoriesId);

            if (currentCategory == null)
            {
                throw new BadRequestException($"No category found with id {CategoriesId}")
                {
                    ErrorCode = "005"
                };

            };
            currentCategory.Name = createCategoryDto.Name;
            var updateCategory = await _categoryRepository.update(currentCategory);

            return _mapper.Map<CategoryResponseDto>(updateCategory);

        }
    }
}
