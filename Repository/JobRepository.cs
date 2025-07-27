using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCVWeb.Util;

namespace Repository
{
    public class JobRepository : IJobRepository
    {
        private readonly MyDbContext _context;

        public JobRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Job job)
        {
            await _context.Jobs.AddAsync(job);
        }

        public void Delete(Job job)
        {
            _context.Jobs.Remove(job);
        }

        public async Task<List<Job>> GetAllAsync()
        {
            return await _context.Jobs.ToListAsync();
        }

        public async Task<Job?> GetByIdAsync(int jobId)
        {
            return await _context.Jobs.FindAsync(jobId);
        }

        public async Task<Job?> GetJobDetailByIdAsync(int jobId)
        {
            return await _context.Jobs
                .Include(j => j.Category)
                .Include(j => j.Recruiter)
                    .ThenInclude(r => r.Company)
                .FirstOrDefaultAsync(j => j.JobId == jobId);
        }

        public async Task<List<Job>> GetJobsByCategoryIdAsync(int categoryId, int excludeJobId, int count)
        {
            return await _context.Jobs
                .Where(j => j.CategoryId == categoryId && j.JobId != excludeJobId)
                .OrderByDescending(j => j.CreateDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Job>> GetJobsByCompanyIdAsync(int companyId, int excludeJobId, int count)
        {
            return await _context.Jobs
                .Where(j => j.Recruiter.CompanyId == companyId && j.JobId != excludeJobId)
                .Include(j => j.Recruiter)
                .OrderByDescending(j => j.CreateDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<PaginatedList<Job>> GetJobsPaginatedAsync(string? searchTitle, int? categoryId, int? companyId, int pageIndex, int pageSize)
        {
            var query = _context.Jobs
                .Include(j => j.Recruiter)
                    .ThenInclude(r => r.Company)
                .Where(j => (j.EndDate == null || j.EndDate > DateOnly.FromDateTime(DateTime.Now)))
                .Where(j => (j.Status == null || j.Status.ToLower() == "active"))
                .Where(j => j.Recruiter != null && j.Recruiter.Company != null && (j.Recruiter.Company.StatusCompany == null || j.Recruiter.Company.StatusCompany == "active"))
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTitle))
            {
                query = query.Where(j => j.JobTitle.Contains(searchTitle));
            }

            if (categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(j => j.CategoryId == categoryId);
            }

            if (companyId.HasValue && companyId > 0)
            {
                query = query.Where(j => j.Recruiter.CompanyId == companyId);
            }

            query = query.OrderByDescending(j => j.CreateDate);
            return await PaginatedList<Job>.CreateAsync(query.AsNoTracking(), pageIndex, pageSize);
        }

        public void Update(Job job)
        {
            _context.Jobs.Update(job);
        }
    }
}
