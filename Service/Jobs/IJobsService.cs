
using DAO.Models;

namespace Service.Jobs
{
    public interface IJobsService
    {
        List<Job> GetJobsByUserId(int userId); 
        Job? GetById(int jobId);                
        void Create(Job job);                 
        void Update(Job job);                  
        void Delete(int jobId);
        List<Job> GetJobsWithApplicantsCountByRecruiterId(int recruiterId);

    }
}
