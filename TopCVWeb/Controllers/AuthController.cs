using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUserService _service;

        public AuthController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user, string rawPassword)
        {
            if (_service.Register(user, rawPassword))
                return RedirectToAction("Login");
            ModelState.AddModelError("", "Tài khoản đã tồn tại!");
            return View(user);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _service.Login(username, password);
            if (user == null)
            {
                ModelState.AddModelError("", "Sai tài khoản hoặc mật khẩu");
                return View();
            }

            HttpContext.Session.SetString("username", user.Username);
            HttpContext.Session.SetInt32("userId", user.UserId);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public IActionResult ForgetPassword(string email)
        {
            var token = _service.GenerateResetToken(email);
            if (token == null)
            {
                ModelState.AddModelError("", "Không tìm thấy email.");
                return View();
            }

            // TODO: Gửi email với link chứa token
            return View("CheckEmail");
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
            if (_service.ResetPassword(token, newPassword))
                return RedirectToAction("Login");
            ModelState.AddModelError("", "Token không hợp lệ hoặc đã hết hạn.");
            return View();
        }
    }
}
