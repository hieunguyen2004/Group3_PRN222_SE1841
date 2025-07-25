using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly MyDbContext _context;

        public CompanyRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
        }

        public async Task<List<Company>> GetAllAsync()
        {
            return await _context.Companies.OrderBy(c => c.CompanyId).ToListAsync();
        }

        public async Task<Company?> GetByIdAsync(int companyId)
        {
            return await _context.Companies.FindAsync(companyId);
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void Update(Company company)
        {
            _context.Companies.Update(company);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
