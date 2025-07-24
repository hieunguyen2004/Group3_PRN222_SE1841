using DAO.Models; 
namespace Service.Applications
{
    public interface IApplicationService
    {
        List<Application> GetApplicantsByJobId(int jobId);
    }
}
