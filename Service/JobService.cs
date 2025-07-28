using DAO.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopCVWeb.Util;
using TopCVWeb.ViewModels;

namespace Service
{
    public class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<HomeViewModel> GetHomeViewModelAsync(string? searchTitle, int? categoryId, int? companyId, int pageIndex, int pageSize, int? seekerId)
        {
            var paginatedJobs = await _unitOfWork.Jobs.GetJobsPaginatedAsync(searchTitle, categoryId, companyId, pageIndex, pageSize);

            var jobViewModels = new List<JobViewModel>();
            foreach (var job in paginatedJobs)
            {
                jobViewModels.Add(new JobViewModel
                {
                    JobId = job.JobId,
                    JobTitle = job.JobTitle,
                    CompanyName = job.Recruiter?.Company?.CompanyName,
                    LogoCompany = job.Recruiter?.Company?.LogoCompany,
                    Salary = job.Salary,
                    Location = job.Location,
                    NumberOfSeeker = job.NumberOfSeeker ?? 0,
                    IsSaved = seekerId.HasValue && await _unitOfWork.SaveJobs.IsJobSavedAsync(seekerId.Value, job.JobId)
                });
            }

            var homeViewModel = new HomeViewModel
            {
                Jobs = new PaginatedList<JobViewModel>(jobViewModels, paginatedJobs.TotalPages, pageIndex, pageSize),
                Categories = await _unitOfWork.Categories.GetAllAsync(),
                Companies = await _unitOfWork.Companies.GetAllAsync(),
                SearchTitle = searchTitle,
                CategoryId = categoryId,
                CompanyId = companyId,
                PageSize = pageSize
            };

            return homeViewModel;
        }

        public async Task<JobDetailViewModel?> GetJobDetailViewModelAsync(int jobId, int? seekerId)
        {
            var job = await _unitOfWork.Jobs.GetJobDetailByIdAsync(jobId);
            if (job == null) return null;

            var sameCategoryJobsTask = await _unitOfWork.Jobs.GetJobsByCategoryIdAsync(job.CategoryId.Value, jobId, 5);
            var sameCompanyJobsTask = await _unitOfWork.Jobs.GetJobsByCompanyIdAsync(job.Recruiter.CompanyId.Value, jobId, 5);
            var isSavedTask = await (seekerId.HasValue ? _unitOfWork.SaveJobs.IsJobSavedAsync(seekerId.Value, jobId) : Task.FromResult(false));

            var sameCategoryJobVms = sameCategoryJobsTask.Select(j => new JobViewModel { JobId = j.JobId, JobTitle = j.JobTitle }).ToList();
            var sameCompanyJobVms = sameCompanyJobsTask.Select(j => new JobViewModel { JobId = j.JobId, JobTitle = j.JobTitle }).ToList();

            var viewModel = new JobDetailViewModel
            {
                JobId = job.JobId,
                JobTitle = job.JobTitle,
                JobDescription = job.JobDescription,
                Requirements = job.Requirements,
                Location = job.Location,
                Position = job.Position,
                Experience = job.Experience,
                Skills = job.Skills,
                Gender = job.Gender,
                Profession = job.Profession,
                JobType = job.JobType,
                NumberOfSeeker = job.NumberOfSeeker ?? 0,
                Salary = job.Salary,
                WorkingTime = job.WorkingTime,
                CreateDate = job.CreateDate ?? null,
                EndDate = job.EndDate ?? null,
                Company = job.Recruiter.Company,
                Category = job.Category,
                IsSaved = isSavedTask,
                JobStatus = job.Status,
                SameCategoryJobs = sameCategoryJobVms,
                SameCompanyJobs = sameCompanyJobVms
            };

            return viewModel;
        }

        public async Task<List<JobViewModel>> GetSavedJobsAsync(int seekerId)
        {
            var savedJobs = await _unitOfWork.SaveJobs.GetSavedJobsBySeekerIdAsync(seekerId);

            return savedJobs.Select(job => new JobViewModel
            {
                JobId = job.JobId,
                JobTitle = job.JobTitle,
                CompanyName = job.Recruiter?.Company?.CompanyName,
                LogoCompany = job.Recruiter?.Company?.LogoCompany,
                Salary = job.Salary,
                Location = job.Location,
                IsSaved = true
            }).ToList();
        }

        public async Task<bool> ToggleSaveJobAsync(int seekerId, int jobId)
        {
            var savedJob = await _unitOfWork.SaveJobs.GetBySeekerAndJobIdAsync(seekerId, jobId);

            if (savedJob != null)
            {
                _unitOfWork.SaveJobs.Remove(savedJob);
                await _unitOfWork.SaveChangesAsync();
                return false;
            }
            else
            {
                var newSave = new SaveJob
                {
                    SeekerId = seekerId,
                    JobId = jobId,
                    SaveDate = DateOnly.FromDateTime(DateTime.UtcNow)
                };
                await _unitOfWork.SaveJobs.AddAsync(newSave);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
        }

        public async Task<int> UpdateJobNumberOfSeeker(int jobId)
        {
            var job = await _unitOfWork.Jobs.GetByIdAsync(jobId);
            if (job != null)
            {
                job.NumberOfSeeker = (job.NumberOfSeeker ?? 0) + 1;
                _unitOfWork.Jobs.Update(job);
                await _unitOfWork.SaveChangesAsync();
            }
            return 1;
        }
    }
}
