using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
namespace E_commerce.application.Interfaces
{
    public interface IProductService
    {
        Task<ProductDto> CreateAsync(CreateProductDto dto);
        Task<ProductDto> Update(int productId, CreateProductDto dto);
        Task<bool> Delete(int id);
        Task<ProductDto> GetById(int productId);
        Task<IEnumerable<ProductDto>> GetAll(ProductQueryDto query);
    }
}
