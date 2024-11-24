using NUnit.Framework;
using Moq;
using BusinessLogic.Concrete;
using DAL.Interface;
using BusinessLogic.Interface;
using DAL.Concrete;
using NUnit.Framework.Legacy;

namespace BusinessLogic_Tests
{
    [TestFixture]
    public class ProductServiceTests
    {
        private Mock<IProductDal> _mockProductDal;
        private IProductService _productService;

        [SetUp]
        public void SetUp()
        {
            _mockProductDal = new Mock<IProductDal>();
            _productService = new ProductService(_mockProductDal.Object);
        }

        [Test]
        public async Task GetAllProductsAsync_ShouldReturnProducts()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { ProductID = 1, ProductName = "Product1", Price = 10 },
                new ProductDto { ProductID = 2, ProductName = "Product2", Price = 20 }
            };

            _mockProductDal.Setup(m => m.GetAllAsync()).ReturnsAsync(products);

            var result = await _productService.GetAllProductsAsync();

            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual("Product1", result[0].ProductName);
            ClassicAssert.AreEqual("Product2", result[1].ProductName);
        }

        [Test]
        public async Task AddProductAsync_ShouldCallInsertOnce()
        {
            var product = new ProductDto { ProductName = "NewProduct", Price = 30 };

            await _productService.AddProductAsync(product);

            _mockProductDal.Verify(m => m.InsertAsync(product), Times.Once);
        }

        [Test]
        public async Task UpdateProductAsync_ShouldCallUpdateOnce()
        {
            var product = new ProductDto { ProductID = 1, ProductName = "UpdatedProduct", Price = 40 };

            await _productService.UpdateProductAsync(product);

            _mockProductDal.Verify(m => m.UpdateAsync(product), Times.Once);
        }

        [Test]
        public async Task RemoveProductAsync_ShouldCallDeleteOnce()
        {
            int productId = 1;

            await _productService.RemoveProductAsync(productId);

            _mockProductDal.Verify(m => m.DeleteAsync(productId), Times.Once);
        }

        [Test]
        public async Task SearchProductsAsync_ShouldReturnFilteredProducts()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { ProductID = 1, ProductName = "Apple", Price = 10 },
                new ProductDto { ProductID = 2, ProductName = "Banana", Price = 20 }
            };

            _mockProductDal.Setup(m => m.GetAllAsync()).ReturnsAsync(products);

            var result = await _productService.SearchProductsAsync("Apple");

            ClassicAssert.AreEqual(1, result.Count);
            ClassicAssert.AreEqual("Apple", result[0].ProductName);
        }

        [Test]
        public async Task SortProductsAsync_ShouldReturnSortedProductsByName()
        {
            var products = new List<ProductDto>
            {
                new ProductDto { ProductID = 1, ProductName = "Banana", Price = 20 },
                new ProductDto { ProductID = 2, ProductName = "Apple", Price = 10 }
            };

            _mockProductDal.Setup(m => m.GetAllAsync()).ReturnsAsync(products);

            var result = await _productService.SortProductsAsync("Name", true);

            ClassicAssert.AreEqual("Apple", result[0].ProductName);
            ClassicAssert.AreEqual("Banana", result[1].ProductName);
        }
    }
}
