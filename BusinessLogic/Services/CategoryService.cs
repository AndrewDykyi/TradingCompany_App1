using DAL.Interface;
using BusinessLogic.Interface;
using DAL.Concrete;

namespace BusinessLogic.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IProductDal _productDal;

        public CategoryService(ICategoryDal categoryDal, IProductDal productDal)
        {
            _categoryDal = categoryDal;
            _productDal = productDal;
        }

        public List<CategoryDto> GetAllCategories()
        {
            return _categoryDal.GetAll();
        }

        public CategoryDto GetCategoryById(int id)
        {
            return _categoryDal.GetById(id);
        }

        public void AddCategory(CategoryDto category)
        {
            _categoryDal.Insert(category);
        }

        public void UpdateCategory(CategoryDto category)
        {
            _categoryDal.Update(category);
        }
        public async Task RemoveCategoryAsync(int id)
        {
            var category = _categoryDal.GetById(id);
            if (category != null)
            {
                category.IsDeleted = true;
                _categoryDal.Update(category);

                var productsInCategory = await _productDal.GetAllAsync();
                var productsToUpdate = productsInCategory.Where(p => p.CategoryID == category.CategoryID).ToList();

                foreach (var product in productsToUpdate)
                {
                    product.Blocked = true;
                    await _productDal.UpdateAsync(product);
                }
            }
        }
    }
}