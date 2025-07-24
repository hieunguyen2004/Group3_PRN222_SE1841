using DAO.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobSeekerService: IJobSeekerService
    {
        private readonly IJobSeekerRepository _repository;

        public JobSeekerService (IJobSeekerRepository repository)
        {
                _repository = repository;
        }

        public async Task<JobSeeker> GetJobSeekerByUserAsync(int? userId)
        {
           return await _repository.GetJobSeekerByUserAsync(userId);
        }
    }
}
