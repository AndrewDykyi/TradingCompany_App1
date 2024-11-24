using DAL.Concrete;

namespace DAL.Interface
{
    public interface IUserDal
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByUsernameAsync(string username);
        Task InsertAsync(UserDto user, string plainPassword);
        Task UpdateAsync(UserDto user, string plainPassword);
        Task DeleteAsync(int id);
    }
}