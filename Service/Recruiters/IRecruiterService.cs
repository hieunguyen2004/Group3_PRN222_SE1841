using DAO.Models;

namespace Repository.Recruiters
{
    public interface IRecruiterRepository
    {
        Recruiter? GetByUserId(int userId);
    }
}
