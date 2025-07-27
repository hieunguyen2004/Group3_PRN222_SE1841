using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCVWeb.ViewModels;

namespace Service.Interface
{
    public interface IJobService
    {
        Task<HomeViewModel> GetHomeViewModelAsync(string? searchTitle, int? categoryId, int? companyId, int pageIndex, int pageSize, int? seekerId);
        Task<JobDetailViewModel?> GetJobDetailViewModelAsync(int jobId, int? seekerId);
        Task<bool> ToggleSaveJobAsync(int seekerId, int jobId);
        Task<List<JobViewModel>> GetSavedJobsAsync(int seekerId);
        void UpdateJobNumberOfSeeker(int jobId);
    }
}
