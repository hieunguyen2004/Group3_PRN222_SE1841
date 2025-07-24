using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ISaveJobRepository
    {
        Task AddAsync(SaveJob saveJob);
        void Remove(SaveJob saveJob);
        Task<bool> IsJobSavedAsync(int seekerId, int jobId);
        Task<SaveJob?> GetBySeekerAndJobIdAsync(int seekerId, int jobId);
        Task<List<Job>> GetSavedJobsBySeekerIdAsync(int seekerId);
    }
}
