using DAO.Models;

namespace Service.Recruiters
{
    public interface IRecruiterService
    {
        Recruiter? GetByUserId(int? userId);
    }
}