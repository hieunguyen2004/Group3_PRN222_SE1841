using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;

namespace Service.Interface
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetAll();
        Category GetById(int id);
        void Add(Category category);
    }
}
