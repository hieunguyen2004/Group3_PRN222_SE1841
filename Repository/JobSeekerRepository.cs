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

        public JobSeekerRepository(MyDbContext context) {
            _context = context; }

        public async Task<JobSeeker> GetJobSeekerByUser(int? userId)
        {
            return await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
        }
    }
}
