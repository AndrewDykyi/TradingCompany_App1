using DAL.Concrete;

namespace BusinessLogic.Interface
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllProductsAsync();
        Task<ProductDto> GetProductByIdAsync(int id);
        Task AddProductAsync(ProductDto product);
        Task UpdateProductAsync(ProductDto product);
        Task RemoveProductAsync(int id);
        Task<List<ProductDto>> SearchProductsAsync(string searchTerm);
        Task<List<ProductDto>> SortProductsAsync(string sortBy, bool ascending);
    }
}