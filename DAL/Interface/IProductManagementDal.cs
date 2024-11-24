using DAL.Concrete;

namespace DAL.Interface
{
    public interface IProductManagementDal
    {
        List<ProductManagementDto> GetAll();
        ProductManagementDto GetById(int id);
        void Insert(ProductManagementDto management);
        void Update(ProductManagementDto management);
        void Delete(int id);
    }
}