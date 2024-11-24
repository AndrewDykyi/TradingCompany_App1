using DAL.Concrete;

namespace BusinessLogic.Interface
{
    public interface IAuthService
    {
        Task<UserDto> AuthenticateAsync(string username, string password);
        UserDto CurrentUser { get; }
    }
}