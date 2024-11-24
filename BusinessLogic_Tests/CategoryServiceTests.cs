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
    public class CategoryServiceTests
    {
        private Mock<ICategoryDal> _mockCategoryDal;
        private Mock<IProductDal> _mockProductDal;
        private ICategoryService _categoryService;

        [SetUp]
        public void SetUp()
        {
            _mockCategoryDal = new Mock<ICategoryDal>();
            _mockProductDal = new Mock<IProductDal>();
            _categoryService = new CategoryService(_mockCategoryDal.Object, _mockProductDal.Object);
        }

        [Test]
        public async Task GetAllCategories_ShouldReturnCategories()
        {
            var categories = new List<CategoryDto>
            {
                new CategoryDto { CategoryID = 1, CategoryName = "Category1" },
                new CategoryDto { CategoryID = 2, CategoryName = "Category2" }
            };

            _mockCategoryDal.Setup(m => m.GetAll()).Returns(categories);

            var result = _categoryService.GetAllCategories();

            ClassicAssert.AreEqual(2, result.Count);
            ClassicAssert.AreEqual("Category1", result[0].CategoryName);
            ClassicAssert.AreEqual("Category2", result[1].CategoryName);
        }

        [Test]
        public void AddCategory_ShouldCallInsertOnce()
        {
            var category = new CategoryDto { CategoryName = "NewCategory" };

            _categoryService.AddCategory(category);

            _mockCategoryDal.Verify(m => m.Insert(category), Times.Once);
        }

        [Test]
        public void UpdateCategory_ShouldCallUpdateOnce()
        {
            var category = new CategoryDto { CategoryID = 1, CategoryName = "UpdatedCategory" };

            _categoryService.UpdateCategory(category);

            _mockCategoryDal.Verify(m => m.Update(category), Times.Once);
        }

        [Test]
        public async Task RemoveCategory_ShouldUpdateCategoryAndBlockProducts()
        {
            var category = new CategoryDto { CategoryID = 1, CategoryName = "CategoryToDelete" };
            var products = new List<ProductDto>
            {
                new ProductDto { ProductID = 1, CategoryID = 1, Blocked = false },
                new ProductDto { ProductID = 2, CategoryID = 1, Blocked = false }
            };

            _mockCategoryDal.Setup(m => m.GetById(1)).Returns(category);
            _mockProductDal.Setup(m => m.GetAllAsync()).ReturnsAsync(products);

            await _categoryService.RemoveCategoryAsync(1);

            _mockCategoryDal.Verify(m => m.Update(It.Is<CategoryDto>(c => c.IsDeleted == true)), Times.Once);
            _mockProductDal.Verify(m => m.UpdateAsync(It.Is<ProductDto>(p => p.Blocked == true)), Times.Exactly(2));
        }
    }
}
