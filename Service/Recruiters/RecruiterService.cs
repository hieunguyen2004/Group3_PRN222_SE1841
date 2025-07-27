using DAO.Models;
using Repository.Recruiters;
using Service.Interface;

namespace Service.Recruiters
{
    public class RecruiterService : IRecruiterService
    {
        private readonly IRecruiterRepository _recruiterRepository;

        public RecruiterService(IRecruiterRepository recruiterRepository)
        {
            _recruiterRepository = recruiterRepository;
        }

        public void Add(Recruiter recruiter)
        {
            _recruiterRepository.Add(recruiter);
        }

        public List<Recruiter> GetAll()
        {
            return _recruiterRepository.GetAll();
        }

        public Recruiter? GetByUserId(int? userId)
        {
            return _recruiterRepository.GetByUserId(userId);
        }
    }
}
