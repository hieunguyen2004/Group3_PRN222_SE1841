using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICVService
    {
        Task AddCVAsync(Cv cv);
        Task<Cv> GetCVByIdAsync(int id);

        Task<bool> ExistsByContentAsync(byte[] content);
        Task<List<Cv>> GetCVsBySeekerIdAsync(int seekerId);

    }
}
