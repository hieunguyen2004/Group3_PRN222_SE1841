using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO.Models;
using Repository.Interface;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext _context;

        public UserRepository(MyDbContext context)
        {
            _context = context;
        }

        public User? GetByUsername(string username) => _context.Users.FirstOrDefault(u => u.Username == username);
        public User? GetByEmail(string email) => _context.Users.FirstOrDefault(u => u.Email == email);
        public User? GetByResetToken(string token)
        {
            return _context.Users.FirstOrDefault(u => u.ResetToken == token);
        }
        public void Add(User user) => _context.Users.Add(user);
        public void Update(User user) => _context.Users.Update(user);
        public void Save() => _context.SaveChanges();
    }

}
