
using DAO.Models;

namespace Repository.Cvs
{
    public class CvRepository : ICvRepository
    {
        private readonly MyDbContext _context;

        public CvRepository(MyDbContext context)
        {
            _context = context;
        }

        public Cv? GetById(int cvId)
        {
            return _context.Cvs.FirstOrDefault(c => c.CvId == cvId);
        }
        public void Update(Cv cv) 
        {
            _context.Cvs.Update(cv);
            _context.SaveChanges();
        }
    }
}
