using DAO.Models;

namespace Repository.Jobs
{

    public interface IJobsRepository
    {
        Job? GetByRecruitmentId(int recruitmentId);
        List<Job> GetJobsByUserId(int? userId);
        Job? GetById(int jobId);
        void Create(Job job);
        void Update(Job job);
        void DeleteJob(Job job);
        List<Job> GetJobsWithApplicantsCountByRecruiterId(int recruiterId);
    }

}
