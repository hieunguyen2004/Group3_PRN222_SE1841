using DAO.Models;
using Repository.Jobs;

namespace Service.Jobs
{
    public class JobsService : IJobsService
    {
        private readonly IJobsRepository _jobsRepo;

        public JobsService(IJobsRepository jobsRepo)
        {
            _jobsRepo = jobsRepo;
        }

        public List<Job> GetJobsByUserId(int? userId)
        {
            return _jobsRepo.GetJobsByUserId(userId);
        }

        public Job? GetById(int jobId)
        {
            return _jobsRepo.GetById(jobId);
        }

        public void Create(Job job)
        {
            _jobsRepo.Create(job);
        }

        public void Update(Job job)
        {
            _jobsRepo.Update(job);
        }

        public void Delete(int jobId)
        {
            var job = _jobsRepo.GetById(jobId);
            if (job != null)
            {
                _jobsRepo.DeleteJob(job); 
            }
        }

        public List<Job> GetJobsWithApplicantsCountByRecruiterId(int recruiterId)
        {
            return _jobsRepo.GetJobsWithApplicantsCountByRecruiterId(recruiterId);
        }
    }
}
