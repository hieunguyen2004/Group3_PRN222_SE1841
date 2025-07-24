using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICompanyRepository
    {
        Task<Company?> GetByIdAsync(int companyId);
        Task<List<Company>> GetAllAsync();
        Task AddAsync(Company company);
        void Update(Company company);
        void Delete(Company company);
    }
}
