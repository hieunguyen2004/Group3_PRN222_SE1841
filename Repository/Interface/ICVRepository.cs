using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ICVRepository
    {
       void Add(Cv cv);
       Cv GetById(int id);
       bool ExistsByContent(byte[] content);
        List<Cv> GetCVsBySeekerId(int seekerId);

    }
}
