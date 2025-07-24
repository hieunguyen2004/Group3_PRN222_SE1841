using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(int categoryId);
        Task<List<Category>> GetAllAsync();
        Task AddAsync(Category category);
        void Update(Category category);
        void Delete(Category category);
    }
}
