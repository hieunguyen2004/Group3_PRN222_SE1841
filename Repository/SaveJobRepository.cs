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
    public class SaveJobRepository : ISaveJobRepository
    {
        private readonly MyDbContext _context;

        public SaveJobRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SaveJob saveJob)
        {
            await _context.SaveJobs.AddAsync(saveJob);
        }

        public async Task<SaveJob?> GetBySeekerAndJobIdAsync(int seekerId, int jobId)
        {
            return await _context.SaveJobs.FirstOrDefaultAsync(sj => sj.SeekerId == seekerId && sj.JobId == jobId);
        }

        public async Task<List<Job>> GetSavedJobsBySeekerIdAsync(int seekerId)
        {
            return await _context.SaveJobs
                .Where(sj => sj.SeekerId == seekerId)
                .Include(sj => sj.Job)
                .ThenInclude(j => j.Recruiter)
                .ThenInclude(r => r.Company)
                .Select(sj => sj.Job)
                .OrderByDescending(j => j.CreateDate)
                .ToListAsync();
        }

        public async Task<bool> IsJobSavedAsync(int seekerId, int jobId)
        {
            return await _context.SaveJobs.AnyAsync(sj => sj.SeekerId == seekerId && sj.JobId == jobId);
        }

        public void Remove(SaveJob saveJob)
        {
            _context.SaveJobs.Remove(saveJob);
        }
    }
}
