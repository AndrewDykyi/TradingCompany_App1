using BusinessLogic.Interface;
using DAL.Concrete;
using DAL.Interface;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLogic.Concrete
{
    public class AuthService : IAuthService
    {
        private readonly IUserDal _userDal;

        public UserDto CurrentUser { get; private set; }

        public AuthService(IUserDal userDal)
        {
            _userDal = userDal;
        }

        public byte[] HashPassword(string password, byte[] salt)
        {
            using (var hmac = new HMACSHA512(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                return hmac.ComputeHash(passwordBytes);
            }
        }

        public byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[32];
                rng.GetBytes(salt);
                return salt;
            }
        }

        public (byte[] PasswordHash, byte[] PasswordSalt) CreatePasswordHash(string password)
        {
            byte[] salt = GenerateSalt();
            byte[] passwordHash = HashPassword(password, salt);
            return (passwordHash, salt);
        }

        public async Task<UserDto> AuthenticateAsync(string username, string password)
        {
            CurrentUser = await GetUserAsync(username, password);
            return CurrentUser;
        }

        private async Task<UserDto> GetUserAsync(string username, string password)
        {
            var user = await _userDal.GetByUsernameAsync(username);
            if (user != null)
            {
                var hashedInputPassword = HashPassword(password, user.PasswordSalt);

                if (AreByteArraysEqual(hashedInputPassword, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }

        private bool AreByteArraysEqual(byte[] a, byte[] b)
        {
            if (a.Length != b.Length) return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i]) return false;
            }

            return true;
        }
    }
}