
using DAO.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Recruiters
{
    public class RecruiterRepository : IRecruiterRepository
    {
        private readonly MyDbContext _context;

        public RecruiterRepository(MyDbContext context)
        {
            _context = context;
        }

        public Recruiter? GetByUserId(int? userId)
        {
            return _context.Recruiters
                           .Include(r => r.User)
                           .FirstOrDefault(r => r.UserId == userId);
        }
    }

}
