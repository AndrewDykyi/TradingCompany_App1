using DAL.Concrete;

namespace DAL.Interface
{
    public interface ICategoryManagementDal
    {
        List<CategoryManagementDto> GetAll();
        CategoryManagementDto GetById(int id);
        void Insert(CategoryManagementDto management);
        void Update(CategoryManagementDto management);
        void Delete(int id);
    }
}