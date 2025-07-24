using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class JobSeekerService : IJobSeekerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public JobSeekerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<int?> GetSeekerIdFromUserIdAsync(int? userId)
        {
            if (!userId.HasValue)
            {
                return null;
            }

            var jobSeeker = await _unitOfWork.JobSeekers.GetByUserIdAsync(userId.Value);
            return jobSeeker?.SeekerId;
        }
    }
}
