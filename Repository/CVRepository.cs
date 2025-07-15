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

        public async Task AddAsync(Cv cv)
        {
            _context.Cvs.Add(cv);
            await _context.SaveChangesAsync();
        }

        public async Task<Cv> GetByIdAsync(int id)
        {
            return await _context.Cvs.FindAsync(id);
        }
        public async Task<bool> ExistsByContentAsync(byte[] content)
        {
            return await _context.Cvs
                .AnyAsync(cv => cv.CvLink.SequenceEqual(content));
        }
        public async Task<List<Cv>> GetCVsBySeekerIdAsync(int seekerId)
        {
            return await _context.Cvs
                .Where(cv => cv.SeekerId == seekerId)
                .OrderByDescending(cv => cv.CvId)
                .ToListAsync();
        }
    }

}
