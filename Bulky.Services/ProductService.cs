using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public List<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
        }

        public Product? GetProductById(int id)
        {
            return _unitOfWork.Product.Get(u => u.Id == id);
        }

        public ProductVM GetProductViewModel(int? id)
        {
            var productVM = new ProductVM
            {
                CategoryList = GetCategorySelectList(),
                Product = new Product()
            };

            if (id != null && id != 0)
            {
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                if (productVM.Product == null)
                {
                    productVM.Product = new Product();
                }
            }

            return productVM;
        }

        public ServiceResult UpsertProduct(ProductVM productVM, IFormFile? file)
        {
            try
            {
                if (file != null)
                {
                    var imageResult = HandleImageUpload(productVM, file);
                    if (!imageResult.Success)
                    {
                        return imageResult;
                    }
                }

                // Create or Update product
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                    _unitOfWork.Save();
                    return ServiceResult.SuccessResult("Product created successfully");
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                    return ServiceResult.SuccessResult("Product updated successfully");
                }
            }
            catch (Exception ex)
            {
                return ServiceResult.ErrorResult($"Error processing product: {ex.Message}");
            }
        }

        public ServiceResult DeleteProduct(int id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(u => u.Id == id);
                if (product == null)
                {
                    return ServiceResult.ErrorResult("Product not found");
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    DeleteImageFile(product.ImageUrl);
                }

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();

                return ServiceResult.SuccessResult("Product deleted successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult.ErrorResult($"Error deleting product: {ex.Message}");
            }
        }

        public IEnumerable<SelectListItem> GetCategorySelectList()
        {
            return _unitOfWork.Category.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            });
        }

        #region Private Helper Methods

        private ServiceResult HandleImageUpload(ProductVM productVM, IFormFile file)
        {
            try
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                string productPath = Path.Combine(wwwRootPath, @"images\product");

                if (!Directory.Exists(productPath))
                {
                    Directory.CreateDirectory(productPath);
                }

                if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                {
                    DeleteImageFile(productVM.Product.ImageUrl);
                }

                using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                productVM.Product.ImageUrl = @"\images\product\" + fileName;
                return ServiceResult.SuccessResult("Image uploaded successfully");
            }
            catch (Exception ex)
            {
                return ServiceResult.ErrorResult($"Error uploading image: {ex.Message}");
            }
        }

        private void DeleteImageFile(string imageUrl)
        {
            try
            {
                var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\'));
                if (File.Exists(oldImagePath))
                {
                    File.Delete(oldImagePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to delete image file: {ex.Message}");
            }
        }

        #endregion
    }
}