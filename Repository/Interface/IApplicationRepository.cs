using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IApplicationRepository
    {
        Task <Application> AddApplication(int jobId, int cvId);
    }
}
