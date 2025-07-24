using DAO.Models;
using Repository.Applications;

namespace Service.Applications
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationsRepository _applicationsRepository;

        public ApplicationService(IApplicationsRepository applicationsRepository)
        {
            _applicationsRepository = applicationsRepository;
        }

        public List<Application> GetApplicantsByJobId(int jobId)
        {
            return _applicationsRepository.GetApplicantsByJobId(jobId);
        }
    }
}
