using TradingCompany_WEB.Controllers;
using Microsoft.AspNetCore.Http;
using TradingCompany_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using NUnit.Framework.Legacy;
using NUnit.Framework;
using DAL.Concrete;
using Moq;

namespace TradingCompanyWEB_Tests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<IAuthService> _authServiceMock;
        private Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private AccountController _controller;

        [SetUp]
        public void Setup()
        {
            _authServiceMock = new Mock<IAuthService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _controller = new AccountController(_authServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };
        }

        [Test]
        public async Task Login_ValidUser_RedirectsToHome()
        {
            var loginModel = new LoginModel { Username = "test", Password = "password" };
            _authServiceMock.Setup(x => x.AuthenticateAsync("test", "password"))
                .ReturnsAsync(new UserDto { Username = "test" });

            var result = await _controller.Login(loginModel) as RedirectToActionResult;

            ClassicAssert.AreEqual("Index", result.ActionName);
            ClassicAssert.AreEqual("Home", result.ControllerName);
        }

        [Test]
        public async Task Login_InvalidUser_ReturnsViewWithErrorMessage()
        {
            var loginModel = new LoginModel { Username = "test", Password = "wrongpassword" };
            _authServiceMock.Setup(x => x.AuthenticateAsync("test", "wrongpassword"))
                .ReturnsAsync((UserDto)null);

            var result = await _controller.Login(loginModel) as ViewResult;
            var returnedModel = result.Model as LoginModel;

            ClassicAssert.IsNotNull(returnedModel);
            ClassicAssert.AreEqual("Invalid username or password", returnedModel.ErrorMessage);
        }

        [Test]
        public async Task Logout_RedirectsToLogin()
        {
            var result = await _controller.Logout() as RedirectToActionResult;

            ClassicAssert.AreEqual("Login", result.ActionName);
        }
    }
}
