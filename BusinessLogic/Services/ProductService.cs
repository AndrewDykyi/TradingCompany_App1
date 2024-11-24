using BusinessLogic.Interface;
using DAL.Concrete;
using DAL.Interface;

namespace BusinessLogic.Concrete
{
    public class ProductService : IProductService
    {
        private readonly IProductDal _productDal;

        public ProductService(IProductDal productDal)
        {
            _productDal = productDal;
        }

        public async Task<List<ProductDto>> GetAllProductsAsync()
        {
            return await _productDal.GetAllAsync();
        }

        public async Task<ProductDto> GetProductByIdAsync(int id)
        {
            return await _productDal.GetByIdAsync(id);
        }

        public async Task AddProductAsync(ProductDto product)
        {
            await _productDal.InsertAsync(product);
        }

        public async Task UpdateProductAsync(ProductDto product)
        {
            await _productDal.UpdateAsync(product);
        }

        public async Task RemoveProductAsync(int id)
        {
            await _productDal.DeleteAsync(id);
        }

        public async Task<List<ProductDto>> SearchProductsAsync(string searchTerm)
        {
            var allProducts = await _productDal.GetAllAsync();
            return allProducts
                .Where(p => p.ProductName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<ProductDto>> SortProductsAsync(string sortBy, bool ascending)
        {
            var products = await _productDal.GetAllAsync();
            switch (sortBy)
            {
                case "Name":
                    return ascending ? products.OrderBy(p => p.ProductName).ToList() : products.OrderByDescending(p => p.ProductName).ToList();
                case "Price":
                    return ascending ? products.OrderBy(p => p.Price).ToList() : products.OrderByDescending(p => p.Price).ToList();
                default:
                    return products;
            }
        }
    }
}