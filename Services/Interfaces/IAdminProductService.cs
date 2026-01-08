using cl_be.Models.Dto.CustomerDto;
using cl_be.Models.Dto.ProductDto;
using cl_be.Models.Dto.ProductDto.Admin;
using cl_be.Models.Pagination;

namespace cl_be.Services.Interfaces
{
    public interface IAdminProductService
    {
        // get: la lista dei prodotti paginati
        Task<Page<AdminProductListDto>> GetAllProductsAsync(
            int page, 
            int pageSize, 
            string? sortBy, 
            string? sortDirection, 
            string? search=null
            );

        Task<AdminProductDetailDto> GetProductDetailsAsync(int productId);

        // helpers: per avere la lista delle categorie e modelli
        Task<IEnumerable<AdminProductCategoryDto>> GetCategoriesAsync();
        Task<IEnumerable<AdminProductModelDto>> GetModelsAsync();

        // edit -> update
        Task<AdminProductEditDto> GetProductToEditAsync(int productId);
        Task UpdateProductAsync(AdminProductUpdateDto dto);

        // create
        Task<int> CreateProductAsync(AdminProductCreateDto dto);

        // delete
        Task DeleteProductAsync(int productId);
    }
}
