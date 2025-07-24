
using DAO.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository.Applications
{
    public class ApplicationsRepository : IApplicationsRepository
    {
        private readonly MyDbContext _context;

        public ApplicationsRepository(MyDbContext context)
        {
            _context = context;
        }

        public List<Application> GetApplicantsByJobId(int jobId)
        {
            return _context.Applications
                .Include(a => a.Cv)                              
                .ThenInclude(c => c.Seeker)                  
                .ThenInclude(s => s.User)            
                .Where(a => a.JobId == jobId)
                .ToList();
        }

    }
}
