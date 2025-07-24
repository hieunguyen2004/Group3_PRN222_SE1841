using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IJobSeekerService
    {
        Task<int?> GetSeekerIdFromUserIdAsync(int? userIdString);
    }
}
