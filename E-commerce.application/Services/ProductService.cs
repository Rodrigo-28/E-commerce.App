using AutoMapper;
using E_commerce.application.Dtos.Requests;
using E_commerce.application.Dtos.Response;
using E_commerce.application.Exceptions;
using E_commerce.application.Interfaces;
using E_commerce.Domain.Interfaces;
using E_commerce.Domain.Models;

namespace E_commerce.application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly ICategoryService _categoryService;

        public ProductService(IProductRepository productRepository, IMapper mapper, ICategoryService categoryService)
        {
            this._productRepository = productRepository;
            this._mapper = mapper;
            this._categoryService = categoryService;
        }
        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var prod = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock = dto.Stock,
                CategoryId = dto.CategoryId,
            };
            var created = await _productRepository.Create(prod);
            var cat = await _categoryService.GetOne(dto.CategoryId);

            var result = _mapper.Map<ProductDto>(created);
            result.CategoryName = cat.Name;
            return result;


        }

        public async Task<bool> Delete(int id)
        {
            var product = await _productRepository.GetById(id);

            if (product == null)
            {
                throw new BadRequestException("product does not exist")
                {
                    ErrorCode = "005"
                };
            }
            return await _productRepository.Delete(product);
        }



        public async Task<IEnumerable<ProductDto>> GetAll(ProductQueryDto query)
        {

            var filter = new ProductFilter
            {
                CategoryId = query.CategoryId,
                MinPrice = query.MinPrice,
                MaxPrice = query.MaxPrice,
                InStockOnly = query.InStockOnly ?? false,
                SearchTerm = query.SearchTerm,

            };

            var allMatched = await _productRepository.GetByFiltersAsync(filter);

            var items = _mapper.Map<IEnumerable<ProductDto>>(allMatched);

            return items;









        }

        public async Task<ProductDto> GetById(int productId)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null)
            {
                throw new NotFoundException($"No product found with id of {productId}")
                {
                    ErrorCode = "004"
                };
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> Update(int productId, CreateProductDto dto)
        {
            var product = await _productRepository.GetById(productId);

            if (product == null)
            {
                throw new BadRequestException($"product car found with id {productId}")
                {
                    ErrorCode = "005"
                };
            }
            product.Name = dto.Name;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.CategoryId = dto.CategoryId;


            var updateProduct = await _productRepository.Update(product);

            return _mapper.Map<ProductDto>(updateProduct);
        }
    }
}
