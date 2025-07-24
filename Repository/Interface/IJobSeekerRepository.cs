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
    }
}
