using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;

namespace E_commerce.application.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> GetOne(int categoryId);
        Task<IEnumerable<CategoryResponseDto>> GetAll();
        Task<CategoryResponseDto> Crete(CreateCategoryDto createCategoryDto);

        Task<CategoryResponseDto> update(int CategoriesId, CreateCategoryDto createCategoryDto);

        Task<bool> Delete(int categoryId);

    }
}
