using DAL.Concrete;

namespace DAL.Interface
{
    public interface IActionDal
    {
        List<ActionDto> GetAll();
        ActionDto GetById(int id);
        void Insert(ActionDto action);
        void Update(ActionDto action);
        void Delete(int id);
    }
}