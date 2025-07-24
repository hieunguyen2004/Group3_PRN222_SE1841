using DAO.Models;
using Repository.Recruiters;

namespace Service.Recruiters
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public RecruiterService(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        public Recruiter? GetByUserId(int userId) => _recruiterRepository.GetByUserId(userId);

    }
}