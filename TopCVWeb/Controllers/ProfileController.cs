using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            var user = _userService.GetUserById(userId.Value);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult Index(User model)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null) return RedirectToAction("Login", "Auth");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _userService.GetUserById(userId.Value);
            if (user == null) return NotFound();

            // Update fields (only editable ones, not password/id)
            user.Firstname = model.Firstname;
            user.Lastname = model.Lastname;
            user.Email = model.Email;
            user.DateOfBirth = model.DateOfBirth;
            user.Gender = model.Gender;
            user.Phone = model.Phone;
            user.Address = model.Address;
            user.Avatar = model.Avatar;

            _userService.Update(user);

            TempData["Message"] = "Profile updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
