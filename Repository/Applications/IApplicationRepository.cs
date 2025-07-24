using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;

namespace Repository.Applications
{
    public interface IApplicationsRepository
    {
        List<Application> GetApplicantsByJobId(int jobId);
       
    }
}
