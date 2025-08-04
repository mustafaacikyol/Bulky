using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Services.IServices;

namespace BulkyBook.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetAll()
        {
            try
            {
                return _unitOfWork.Category.GetAll().ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (ex) as needed
                throw new Exception("An error occurred while retrieving categories.", ex);
            }
        }

        public Boolean Create(Category obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Category object cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(obj.Name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(obj.Name));
            }
            if (obj.DisplayOrder <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(obj.DisplayOrder), "Display order must be greater than zero.");
            }
            try
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the category.", ex);
            }
        }

        public Category GetById(int id)
        {
            try
            {
                Category? categoryFromDb = _unitOfWork.Category.Get(u => u.Id == id);
                return categoryFromDb;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while retrieving the category with ID {id}.", ex);
            }
        }

        public Boolean Update(Category obj)
        {
            try
            {
                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An error occurred while updating the category.", ex);
            }
        }

        public Boolean Delete(int id)
        {
            try
            {
                Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
                if (obj == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found.");
                }
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception("An error occurred while deleting the category.", ex);
            }
        }
    }
}
