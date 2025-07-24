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
        bool Register(User user, string rawPassword, string role, out string error);

        User? Login(string username, string password);
        string? GenerateResetToken(string email);
        bool ResetPassword(string token, string newPassword);
        User? GetUserByEmail(string email);
        void SendResetPasswordEmail(string email, string resetLink);
    }
}
