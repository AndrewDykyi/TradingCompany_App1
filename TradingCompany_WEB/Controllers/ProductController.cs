using AutoMapper;
using BusinessLogic.Interface;
using DAL.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TradingCompany_WEB.Models;

namespace TradingCompany_WEB.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var productsDto = await _productService.GetAllProductsAsync();
            var productsModel = _mapper.Map<List<ProductModel>>(productsDto);
            return View(productsModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            var productModel = _mapper.Map<ProductModel>(productDto);
            return View(productModel);
        }

        public IActionResult Add()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(ProductModel productModel)
        {
            if (ModelState.IsValid)
            {
                var productDto = _mapper.Map<ProductDto>(productModel);
                await _productService.AddProductAsync(productDto);
                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }

            var productModel = _mapper.Map<ProductModel>(productDto);
            return View(productModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductModel productModel)
        {
            if (id != productModel.ProductID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var productDto = _mapper.Map<ProductDto>(productModel);
                try
                {
                    await _productService.UpdateProductAsync(productDto);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Не вдалося зберегти зміни.");
                    return View(productModel);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(productModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var productDto = await _productService.GetProductByIdAsync(id);
            if (productDto == null)
            {
                return NotFound();
            }

            var productModel = _mapper.Map<ProductModel>(productDto);
            return View(productModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productService.RemoveProductAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}