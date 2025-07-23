using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICVRepository
    {
        Task AddAsync(Cv cv);
        Task<Cv> GetByIdAsync(int id);
        Task<bool> ExistsByContentAsync(byte[] content);
        Task<List<Cv>> GetCVsBySeekerIdAsync(int seekerId);

    }
}
