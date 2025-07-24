using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IJobRepository Jobs { get; }
        ICategoryRepository Categories { get; }
        ICompanyRepository Companies { get; }
        ISaveJobRepository SaveJobs { get; }
        IJobSeekerRepository JobSeekers { get; }

        Task<int> SaveChangesAsync();
    }
}
