
using DAO.Models;
using Repository.Cvs;

namespace Service.Cvs
{
    public class CvService : ICvService
    {
        private readonly ICvRepository _cvRepository;

        public CvService(ICvRepository cvRepository)
        {
            _cvRepository = cvRepository;
        }

        public Cv? GetById(int cvId)
        {
            return _cvRepository.GetById(cvId);
        }
    }
}
