using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IJobSeekerRepository
    {
        JobSeeker GetJobSeekerByUser(int? userId);
        Task<JobSeeker?> GetByIdAsync(int seekerId);
        Task<JobSeeker?> GetByUserIdAsync(int userId);
        Task AddAsync(JobSeeker jobSeeker);
        void Update(JobSeeker jobSeeker);
        void Delete(JobSeeker jobSeeker);
    }
}
