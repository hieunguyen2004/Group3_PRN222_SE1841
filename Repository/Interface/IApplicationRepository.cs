using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IApplicationRepository
    {
       Application AddApplication(int jobId, int cvId);
        List<Application> GetApplicantsByJobId(int jobId);

        Job? GetJobByApplicationId(int applicationId);
        Company? GetCompanyByRecruiterId(int recruiterId);

        int GetRecruiterIdFromUserId(int userId);
    }
}
