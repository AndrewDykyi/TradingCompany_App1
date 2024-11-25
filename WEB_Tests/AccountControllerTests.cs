using TradingCompany_WEB.Controllers;
using TradingCompany_WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using DAL.Concrete;
using Moq;
using NUnit.Framework.Legacy;

namespace WEB_Tests
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

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Login_InvalidUser_ReturnsViewWithErrorMessage()
        {
            var loginModel = new LoginModel { Username = "test", Password = "wrongpassword" };
            _authServiceMock.Setup(x => x.AuthenticateAsync("test", "wrongpassword"))
                .ReturnsAsync((UserDto)null);

            var result = await _controller.Login(loginModel) as ViewResult;
            var returnedModel = result.Model as LoginModel;

            ClassicAssert.IsNotNull(result);
            ClassicAssert.IsNotNull(returnedModel);
            ClassicAssert.AreEqual("Invalid username or password", returnedModel.ErrorMessage);
        }
    }
}