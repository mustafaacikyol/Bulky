using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductService
    {
        List<Product> GetAllProducts();

        Product? GetProductById(int id);

        ProductVM GetProductViewModel(int? id);

        ServiceResult UpsertProduct(ProductVM productVM, IFormFile? file);

        ServiceResult DeleteProduct(int id);

        IEnumerable<SelectListItem> GetCategorySelectList();
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public static ServiceResult SuccessResult(string message, object? data = null)
        {
            return new ServiceResult { Success = true, Message = message, Data = data };
        }

        public static ServiceResult ErrorResult(string message)
        {
            return new ServiceResult { Success = false, Message = message };
        }
    }
}