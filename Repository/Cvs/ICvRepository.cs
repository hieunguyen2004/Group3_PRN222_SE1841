
using DAO.Models;

namespace Repository.Cvs
{
    public interface ICvRepository
    {
        Cv? GetById(int cvId);
    }
}
