using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ICVService
    {
        void AddCV(Cv cv);
        Cv GetCVById(int id);

        bool ExistsByContent(byte[] content);
        List<Cv> GetCVsBySeekerId(int seekerId);

        void ConfirmCv(int cvId);

        void RejectCv(int cvId);

    }
}
