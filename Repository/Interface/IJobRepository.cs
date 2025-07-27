using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCVWeb.Util;

namespace Repository.Interface
{
    public interface IJobRepository
    {
        Task<Job?> GetByIdAsync(int jobId);
        Task<List<Job>> GetAllAsync();
        Task AddAsync(Job job);
        void Update(Job job);
        void Delete(Job job);

        Task<PaginatedList<Job>> GetJobsPaginatedAsync(string? searchTitle, int? categoryId, int? companyId, int pageIndex, int pageSize);
        Task<Job?> GetJobDetailByIdAsync(int jobId);
        Task<List<Job>> GetJobsByCategoryIdAsync(int categoryId, int excludeJobId, int count);
        Task<List<Job>> GetJobsByCompanyIdAsync(int companyId, int excludeJobId, int count);
    }
}