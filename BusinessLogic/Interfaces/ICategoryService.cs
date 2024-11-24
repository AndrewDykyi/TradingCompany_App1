using DAL.Concrete;

namespace BusinessLogic.Interface
{
    public interface ICategoryService
    {
        List<CategoryDto> GetAllCategories();
        CategoryDto GetCategoryById(int id);
        void AddCategory(CategoryDto category);
        void UpdateCategory(CategoryDto category);
        Task RemoveCategoryAsync(int id);
    }
}