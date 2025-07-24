using DAO.Models;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly MyDbContext _context;

        public ApplicationRepository(MyDbContext context)
        {
            _context = context;
        }

        public Application AddApplication(int jobId, int cvId)
        {
            var application = new Application
            {
                CvId = cvId,          
                JobId = jobId,               
                SubmitDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Status = "Pending"
            };

            _context.Applications.Add(application);
        
             _context.SaveChangesAsync();

            return application;

        }
    }
}
