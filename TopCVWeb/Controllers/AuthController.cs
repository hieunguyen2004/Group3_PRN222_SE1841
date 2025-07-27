using DAO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using TopCVWeb.Models;

namespace TopCVWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _userService;

        public AuthController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet]
        public IActionResult Register() => View();

        public IActionResult Register(User user, string rawPassword)
        {
            string role = "jobseeker";
            if (!_userService.Register(user, rawPassword, role, out string error))
            {
                ViewBag.Error = error;
                return View(user); 
            }

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login(string? username = "")
        {
            ViewBag.Username = username ?? "";
            return View();
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _userService.Login(username, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View();
            }

            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetInt32("userId", user.UserId);
            HttpContext.Session.SetInt32("RoleId", user.RoleId ?? 0);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgotPassword() => View(); 

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = _userService.GetUserByEmail(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email không tồn tại.");
                return View(model);
            }

            string token = _userService.GenerateResetToken(model.Email);
            string resetLink = Url.Action("ResetPassword", "Auth", new { token = token }, Request.Scheme);

            _userService.SendResetPasswordEmail(user.Email, resetLink);

            ViewBag.Message = "Email đặt lại mật khẩu đã được gửi.";
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            ViewBag.Token = token;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(string token, string newPassword)
        {
            if (_userService.ResetPassword(token, newPassword))
                return RedirectToAction("Login");

            ModelState.AddModelError("", "Token không hợp lệ hoặc đã hết hạn.");

            ViewBag.Token = token; 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Xóa toàn bộ session
            return RedirectToAction("Login", "Auth");
        }
    }
}
