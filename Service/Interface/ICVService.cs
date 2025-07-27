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

        void ConfirmCv(int cvId,string email, int appId, int userId);

        void RejectCv(int cvId,string email, int appId, int userId);

    }
}
