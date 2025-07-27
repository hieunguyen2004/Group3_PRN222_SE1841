using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;

namespace Service.Cvs
{
    public interface ICvService
    {
        Cv? GetById(int cvId);
        void Update(Cv cv);
    }
}
