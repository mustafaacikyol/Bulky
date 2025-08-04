using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Services.IServices
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();

        Boolean Create(Category obj);

        Category GetById(int id);

        Boolean Update(Category obj);

        Boolean Delete(int id);

    }
}
