using DAL.Concrete;

namespace BusinessLogic.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByUsernameAsync(string username);
        Task AddUserAsync(UserDto user, string plainPassword);
        Task UpdateUserAsync(UserDto user, string plainPassword);
        Task RemoveUserAsync(int id);
    }
}