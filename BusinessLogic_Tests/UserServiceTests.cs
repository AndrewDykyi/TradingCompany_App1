using NUnit.Framework;
using Moq;
using BusinessLogic.Concrete;
using DAL.Interface;
using BusinessLogic.Interface;
using DAL.Concrete;
using System;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace BusinessLogic_Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IUserDal> _mockUserDal;
        private IUserService _userService;

        [SetUp]
        public void SetUp()
        {
            _mockUserDal = new Mock<IUserDal>();
            _userService = new UserService(_mockUserDal.Object);
        }

        [Test]
        public async Task GetUserByIdAsync_ShouldReturnUser()
        {
            var user = new UserDto
            {
                UserID = 1,
                Username = "JohnDoe",
                PasswordHash = new byte[] { 1, 2, 3 },
                PasswordSalt = new byte[] { 4, 5, 6 }
            };
            _mockUserDal.Setup(m => m.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(1);

            ClassicAssert.AreEqual(1, result.UserID);
            ClassicAssert.AreEqual("JohnDoe", result.Username);
            ClassicAssert.AreEqual(new byte[] { 1, 2, 3 }, result.PasswordHash);
        }

        [Test]
        public async Task AddUserAsync_ShouldCallInsertOnce()
        {
            var user = new UserDto
            {
                Username = "NewUser",
                PasswordHash = new byte[] { 7, 8, 9 },
                PasswordSalt = new byte[] { 10, 11, 12 }
            };
            var plainPassword = "Password123";

            await _userService.AddUserAsync(user, plainPassword);

            _mockUserDal.Verify(m => m.InsertAsync(user, plainPassword), Times.Once);
        }

        [Test]
        public async Task UpdateUserAsync_ShouldCallUpdateOnce()
        {
            var user = new UserDto
            {
                UserID = 1,
                Username = "UpdatedUser",
                PasswordHash = new byte[] { 13, 14, 15 },
                PasswordSalt = new byte[] { 16, 17, 18 }
            };
            var plainPassword = "UpdatedPassword123";

            await _userService.UpdateUserAsync(user, plainPassword);

            _mockUserDal.Verify(m => m.UpdateAsync(user, plainPassword), Times.Once);
        }

        [Test]
        public async Task RemoveUserAsync_ShouldCallDeleteOnce()
        {
            int userId = 1;

            await _userService.RemoveUserAsync(userId);

            _mockUserDal.Verify(m => m.DeleteAsync(userId), Times.Once);
        }

        [Test]
        public void AddUserAsync_ShouldThrowArgumentNullException_WhenUserIsNull()
        {
            UserDto user = null;
            var plainPassword = "Password123";

            Assert.ThrowsAsync<ArgumentNullException>(async () => await _userService.AddUserAsync(user, plainPassword));
        }

        [Test]
        public void AddUserAsync_ShouldThrowArgumentException_WhenPasswordIsEmpty()
        {
            var user = new UserDto
            {
                Username = "InvalidUser",
                PasswordHash = new byte[] { 19, 20, 21 },
                PasswordSalt = new byte[] { 22, 23, 24 }
            };
            var plainPassword = "";

            Assert.ThrowsAsync<ArgumentException>(async () => await _userService.AddUserAsync(user, plainPassword));
        }
    }
}
