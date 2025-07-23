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

        public async Task AddCVAsync(Cv cv) 
        {
            await _cvRepository.AddAsync(cv);
        }

        public async Task<Cv> GetCVByIdAsync(int id)
        {
            return await _cvRepository.GetByIdAsync(id);
        }
        public async Task<bool> ExistsByContentAsync(byte[] content)
        {
            return await _cvRepository.ExistsByContentAsync(content);
        }
        public async Task<List<Cv>> GetCVsBySeekerIdAsync(int seekerId)
        {
            return await _cvRepository.GetCVsBySeekerIdAsync(seekerId);
        }


    }
}
