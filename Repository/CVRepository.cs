using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class CVRepository : ICVRepository
    {
        private readonly MyDbContext _context;

        public CVRepository(MyDbContext context)
        {
            _context = context;
        }

        public  void Add(Cv cv)
        {
            _context.Cvs.Add(cv);
            _context.SaveChangesAsync();
        }

        public Cv GetById(int id)
        {
            return  _context.Cvs.Find(id);
        }
        public  bool ExistsByContent(byte[] content)
        {
            return  _context.Cvs
                .Any(cv => cv.CvLink.SequenceEqual(content));
        }
        public  List<Cv> GetCVsBySeekerId(int seekerId)
        {
            return  _context.Cvs
                .Include(c=>c.Applications)
                .ThenInclude(a=>a.Job)
                .Where(cv => cv.SeekerId == seekerId)
                .OrderByDescending(cv => cv.CvId)
                .ToList();
        }
    }

}
