using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;
using Service.Interface;
using System.Net;
using System.Net.Mail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly MyDbContext _context;



        public UserService(IUserRepository repo, MyDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public bool Register(User user, string rawPassword, string role, out string error)
        {
            error = "";

            try
            {
                if (_repo.GetByUsername(user.Username) != null)
                {
                    error = "Tên đăng nhập đã tồn tại.";
                    return false;
                }

                if (_repo.GetByEmail(user.Email!) != null)
                {
                    error = "Email đã được sử dụng.";
                    return false;
                }

                user.Password = PasswordHasher.HashPassword(rawPassword);
                _repo.Add(user);

                if (role == "recruiter")
                {
                    var recruiter = new Recruiter
                    {
                        UserId = user.UserId,
                        CompanyEmail = user.Email ?? "",
                        Position2 = "",
                        CompanyId = null
                    };
                    _context.Recruiters.Add(recruiter);
                }

                _repo.Save(); // Save toàn bộ
                return true;
            }
            catch (Exception ex)
            {
                error = "Lỗi khi đăng ký: " + ex.Message;
                return false;
            }
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

            var tokenEntry = new TokenForgetPassword
            {
                Token = token,
                ExpiryTime = DateTime.UtcNow.AddMinutes(30),
                IsUsed = false,
                UserId = user.UserId
            };

            _context.TokenForgetPasswords.Add(tokenEntry);
            _context.SaveChanges();

            return token;
        }

        public void SendResetPasswordEmail(string email, string resetLink)
        {
            var fromEmail = "sontranphamthai@gmail.com";
            var fromPassword = "occy rhrj itvt vdez";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mail = new MailMessage(fromEmail, email)
            {
                Subject = "Đặt lại mật khẩu",
                Body = $"Nhấn vào link sau để đặt lại mật khẩu: {resetLink}",
                IsBodyHtml = false
            };

            smtp.Send(mail);
        }

        public bool ResetPassword(string token, string newPassword)
        {
            var tokenEntry = _context.TokenForgetPasswords
                .Include(t => t.User)
                .FirstOrDefault(t => t.Token == token && t.IsUsed == false);

            if (tokenEntry == null || tokenEntry.ExpiryTime < DateTime.UtcNow)
                return false;

            var user = tokenEntry.User;
            if (user == null) return false;

            user.Password = PasswordHasher.HashPassword(newPassword);
            tokenEntry.IsUsed = true;

            _context.Users.Update(user);
            _context.TokenForgetPasswords.Update(tokenEntry);
            _context.SaveChanges();

            return true;
        }

        public User GetUserById(int userId) => _repo.GetById(userId);
        //public void Update(User user) => _repo.Update(user);

        public User? GetUserByEmail(string email)
        {
            return _repo.GetByEmail(email);
        }
        public List<User> GetAll()
        {
            return _repo.GetAll();
        }

        public User? GetById(int id)
        {
            return _repo.GetById(id);
        }
        public void Update(User user)
        {
            _repo.Update(user);
            _repo.Save();
        }
    }
}
