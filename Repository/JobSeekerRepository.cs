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
    public class JobSeekerRepository : IJobSeekerRepository
    {
        private readonly MyDbContext _context;

        public JobSeekerRepository(MyDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(JobSeeker jobSeeker)
        {
            await _context.JobSeekers.AddAsync(jobSeeker);
        }

        public void Delete(JobSeeker jobSeeker)
        {
            _context.JobSeekers.Remove(jobSeeker);
        }

        public async Task<JobSeeker?> GetByIdAsync(int seekerId)
        {
            return await _context.JobSeekers.FindAsync(seekerId);
        }

        public async Task<JobSeeker?> GetByUserIdAsync(int userId)
        {
            return await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
        }

        public void Update(JobSeeker jobSeeker)
        {
            _context.JobSeekers.Update(jobSeeker);
        }
    }
}
