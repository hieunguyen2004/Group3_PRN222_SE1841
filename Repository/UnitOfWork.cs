using DAO.Models;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MyDbContext _context;
        public IJobRepository Jobs { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public ICompanyRepository Companies { get; private set; }
        public ISaveJobRepository SaveJobs { get; private set; }
        public IJobSeekerRepository JobSeekers { get; private set; }

        public UnitOfWork(MyDbContext context)
        {
            _context = context;
            Jobs = new JobRepository(_context);
            Categories = new CategoryRepository(_context);
            Companies = new CompanyRepository(_context);
            SaveJobs = new SaveJobRepository(_context);
            JobSeekers = new JobSeekerRepository(_context);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
