using TradingCompany_WEB.Controllers;
using TradingCompany_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using BusinessLogic.Interface;
using NUnit.Framework.Legacy;
using DAL.Concrete;
using AutoMapper;
using Moq;

namespace WEB_Tests
{
    [TestFixture]
    public class ProductControllerTests
    {
        private Mock<IProductService> _productServiceMock;
        private Mock<IMapper> _mapperMock;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
            _controller = new ProductController(_productServiceMock.Object, _mapperMock.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [Test]
        public async Task Index_ReturnsViewWithProducts()
        {
            var productDtos = new List<ProductDto>
            {
                new ProductDto { ProductID = 1, ProductName = "Product1", Price = 100 },
                new ProductDto { ProductID = 2, ProductName = "Product2", Price = 200 }
            };
            _productServiceMock.Setup(x => x.GetAllProductsAsync()).ReturnsAsync(productDtos);

            var productModels = new List<ProductModel>
            {
                new ProductModel { ProductID = 1, ProductName = "Product1", Price = 100 },
                new ProductModel { ProductID = 2, ProductName = "Product2", Price = 200 }
            };
            _mapperMock.Setup(m => m.Map<List<ProductModel>>(productDtos)).Returns(productModels);

            var result = await _controller.Index() as ViewResult;

            var model = result.Model as List<ProductModel>;
            ClassicAssert.IsNotNull(model);
            ClassicAssert.AreEqual(2, model.Count);
        }

        [Test]
        public async Task Details_ReturnsProductView()
        {
            var productDto = new ProductDto { ProductID = 1, ProductName = "Product1", Price = 100 };
            _productServiceMock.Setup(x => x.GetProductByIdAsync(1)).ReturnsAsync(productDto);

            var productModel = new ProductModel { ProductID = 1, ProductName = "Product1", Price = 100 };
            _mapperMock.Setup(m => m.Map<ProductModel>(productDto)).Returns(productModel);

            var result = await _controller.Details(1) as ViewResult;

            var model = result.Model as ProductModel;
            ClassicAssert.IsNotNull(model);
            ClassicAssert.AreEqual(1, model.ProductID);
        }

        [Test]
        public async Task DeleteConfirmed_CallsRemoveProductAsyncAndRedirects()
        {
            var result = await _controller.DeleteConfirmed(1) as RedirectToActionResult;

            _productServiceMock.Verify(x => x.RemoveProductAsync(1), Times.Once);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Index", result.ActionName);
        }
    }
}
