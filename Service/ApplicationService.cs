using DAO.Models;
using Repository;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _applicationRepository;

        public ApplicationService(IApplicationRepository applicationRepository)
        {
            _applicationRepository = applicationRepository;
        }

        public Application AddApplication(int jobId, int cvId)
        {
            return _applicationRepository.AddApplication(jobId, cvId);
        }
        public List<Application> GetApplicantsByJobId(int jobId)
        {
            return _applicationRepository.GetApplicantsByJobId(jobId);
        }
    }
}
