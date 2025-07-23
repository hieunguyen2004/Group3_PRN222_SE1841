using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public bool Register(User user, string rawPassword)
        {
            if (_repo.GetByUsername(user.Username) != null) return false;
            user.Password = PasswordHasher.HashPassword(rawPassword);
            _repo.Add(user);
            _repo.Save();
            return true;
        }

        public User? Login(string username, string password)
        {
            var user = _repo.GetByUsername(username);
            if (user == null || !PasswordHasher.VerifyPassword(password, user.Password)) return null;
            return user;
        }

        public string? GenerateResetToken(string email)
        {
            var user = _repo.GetByEmail(email);
            if (user == null) return null;

            string token = Guid.NewGuid().ToString();
            user.ResetToken = token;
            user.TokenExpiry = BitConverter.GetBytes(DateTime.UtcNow.AddMinutes(30).Ticks); // expiry
            _repo.Update(user);
            _repo.Save();

            // TODO: Gửi mail ở đây
            return token;
        }

        public bool ResetPassword(string token, string newPassword)
        {
            var user = _repo.GetByResetToken(token);
            if (user == null || user.TokenExpiry == null) return false;

            long expiryTicks = BitConverter.ToInt64(user.TokenExpiry);
            if (DateTime.UtcNow > new DateTime(expiryTicks)) return false;

            user.Password = PasswordHasher.HashPassword(newPassword);
            user.ResetToken = null;
            user.TokenExpiry = null;
            _repo.Update(user);
            _repo.Save();
            return true;
        }
    }
}
