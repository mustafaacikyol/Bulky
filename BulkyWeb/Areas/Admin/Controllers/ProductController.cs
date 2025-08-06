using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            var productVM = _productService.GetProductViewModel(id);
            return View(productVM);
        }

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                var result = _productService.UpsertProduct(productVM, file);

                if (result.Success)
                {
                    TempData["success"] = result.Message;
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["error"] = result.Message;
                }
            }

            // If we got this far, something failed, redisplay form
            productVM.CategoryList = _productService.GetCategorySelectList();
            return View(productVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _productService.GetAllProducts();
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = _productService.DeleteProduct(id);
            return Json(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        #endregion
    }
}