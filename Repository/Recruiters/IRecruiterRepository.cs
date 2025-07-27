using DAO.Models;

public interface IRecruiterRepository
{
    void Add(Recruiter recruiter);
    List<Recruiter> GetAll();
    Recruiter? GetByUserId(int? userId);
}
