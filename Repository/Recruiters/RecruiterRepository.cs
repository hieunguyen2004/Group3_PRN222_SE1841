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

        public void Add(Recruiter recruiter)
        {
            _context.Recruiters.Add(recruiter);
            _context.SaveChanges();
        }

        public List<Recruiter> GetAll()
        {
            return _context.Recruiters
                           .Include(r => r.User)
                           .Include(r => r.Company)
                           .ToList();
        }

        public Recruiter? GetByUserId(int? userId)
        {
            return _context.Recruiters
                           .Include(r => r.User)
                           .Include(r => r.Company)
                           .FirstOrDefault(r => r.UserId == userId);
        }
    }
}
