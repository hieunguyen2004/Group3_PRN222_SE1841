using DAO.Models;

namespace Service.Interface
{
    public interface IRecruiterService
    {
        void Add(Recruiter recruiter);
        Recruiter? GetByUserId(int? userId);
        List<Recruiter> GetAll();
    }
}
