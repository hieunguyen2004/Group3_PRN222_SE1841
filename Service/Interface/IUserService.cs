using DAO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IUserService
    {
        bool Register(User user, string rawPassword);
        User? Login(string username, string password);
        string? GenerateResetToken(string email);
        bool ResetPassword(string token, string newPassword);
        User GetUserById(int userId);
        void Update(User user);

    }
}
