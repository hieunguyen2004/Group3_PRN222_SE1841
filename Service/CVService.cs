using DAO.Models;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CVService: ICVService
    {
        private readonly ICVRepository _cvRepository;

        public CVService(ICVRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public void AddCV(Cv cv) 
        {
            _cvRepository.Add(cv);
        }

        public  Cv GetCVById(int id)
        {
            return  _cvRepository.GetById(id);
        }
        public  bool ExistsByContent(byte[] content)
        {
            return  _cvRepository.ExistsByContent(content);
        }
        public  List<Cv> GetCVsBySeekerId(int seekerId)
        {
            return  _cvRepository.GetCVsBySeekerId(seekerId);
        }

        public void ConfirmCv(int cvId)
        {
            _cvRepository.ConfirmCv(cvId);
        }

        public void RejectCv(int cvId)
        {
            _cvRepository.RejectCv(cvId);
        }
    }
}
