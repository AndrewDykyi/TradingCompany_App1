using DAL.Concrete;

namespace DAL.Interface
{
    public interface ICategoryDal
    {
        List<CategoryDto> GetAll();
        CategoryDto GetById(int id);
        void Insert(CategoryDto category);
        void Update(CategoryDto category);
        void Delete(int id);
    }
}