using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service.Interface
{
    public interface IApplicationService
    {
        Task<Application> AddApplication(int jobId, int cvId);
    }
}
