using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IUserRepository
    {
        User? GetByUsername(string username);
        User? GetByEmail(string email);
        User? GetByResetToken(string token);
        void Add(User user);
        void Update(User user);
        void Save();
        User GetById(int userId);
        List<User> GetAll();
    }
}
