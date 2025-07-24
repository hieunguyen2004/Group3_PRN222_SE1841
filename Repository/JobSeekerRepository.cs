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

        public JobSeeker GetJobSeekerByUser(int? userId)
        {
            return  _context.JobSeekers.FirstOrDefault(js => js.UserId == userId);
        }
    }
}
