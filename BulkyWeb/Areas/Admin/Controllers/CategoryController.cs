using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.Services;
using BulkyBook.Services.IServices;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Category> CategoryList = _categoryService.GetAll().ToList();
            return View(CategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            Boolean isCreated = _categoryService.Create(obj);
            if (isCreated)
            {
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Failed to create category. Please check the input data.");
                return View();
            }
        }

        public IActionResult Edit(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? categoryFromDb = _categoryService.GetById(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                Boolean isUpdated = _categoryService.Update(obj);
                if(isUpdated)
                {
                    TempData["success"] = "Category updated successfully";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _categoryService.GetById(id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Boolean isDeleted = _categoryService.Delete(id);
            if(isDeleted)
            {
                TempData["success"] = "Category deleted successfully";
                return RedirectToAction("Index");
            }
            return View();

        }


    }
}
