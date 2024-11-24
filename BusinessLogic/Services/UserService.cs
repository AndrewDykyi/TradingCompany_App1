using DAL.Interface;
using BusinessLogic.Interface;
using DAL.Concrete;

namespace BusinessLogic.Concrete
{
    public class UserService : IUserService
    {
        private readonly IUserDal _userDal;

        public UserService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            return await _userDal.GetAllAsync();
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            return await _userDal.GetByIdAsync(id);
        }

        public async Task<UserDto> GetUserByUsernameAsync(string username)
        {
            return await _userDal.GetByUsernameAsync(username);
        }

        public async Task AddUserAsync(UserDto user, string plainPassword)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(plainPassword)) throw new ArgumentException("Password cannot be null or empty.", nameof(plainPassword));

            await _userDal.InsertAsync(user, plainPassword);
        }

        public async Task UpdateUserAsync(UserDto user, string plainPassword)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(plainPassword)) throw new ArgumentException("Password cannot be null or empty.", nameof(plainPassword));

            await _userDal.UpdateAsync(user, plainPassword);
        }

        public async Task RemoveUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentException("Invalid user ID.", nameof(id));

            await _userDal.DeleteAsync(id);
        }
    }
}