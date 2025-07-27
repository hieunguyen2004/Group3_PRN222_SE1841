using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: /Admin/UserList
        public IActionResult UserList(string? searchTerm, int? role, int page = 1, int pageSize = 10)
        {
            try
            {
                var users = _userService.GetAll();

                // Chỉ lấy user có RoleId là 1 hoặc 2
                users = users
                    .Where(u => u.RoleId == 1 || u.RoleId == 2)
                    .ToList();

                // Tìm kiếm
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = users
                        .Where(u =>
                            (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }

                // Lọc theo RoleId
                if (role != null && (role == 1 || role == 2))
                {
                    users = users.Where(u => u.RoleId == role).ToList();
                }

                var totalUsers = users.Count;
                var totalPages = (int)Math.Ceiling(totalUsers / (double)pageSize);

                var pagedUsers = users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = totalPages;
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SelectedRole = role;

                return View(pagedUsers);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi tải danh sách người dùng: {ex.Message}";
                return View(new List<User>());
            }
        }

        // GET: /Admin/UserDetail/{id}
        public IActionResult UserDetail(int id)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng.");
                }
                return View(user);
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Lỗi khi tải thông tin người dùng: {ex.Message}";
                return View();
            }
        }

        // POST: /Admin/ChangeStatus
        [HttpPost]
        public IActionResult ChangeStatus(int id, string newStatus)
        {
            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                    return NotFound("Không tìm thấy người dùng.");

                user.Status = newStatus;
                _userService.Update(user);

                TempData["Message"] = $"Đã cập nhật trạng thái người dùng #{id} thành '{newStatus}'";
                return RedirectToAction("UserList");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi cập nhật trạng thái: {ex.Message}";
                return RedirectToAction("UserList");
            }
        }

        [HttpPost]
        [HttpPost]
        public IActionResult CreateRandomRecruiter()
        {
            var rand = new Random();
            string username;
            string email;
            string rawPassword = "123456";
            string hashedPassword = "jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=";
            User user;

            while (true)
            {
                var randomNumber = rand.Next(1000, 9999);
                username = $"recruiter{randomNumber}";
                email = $"recruiter{randomNumber}@example.com";

                var existed = _userService.GetAll().Any(u =>
                    u.Username == username || u.Email == email);

                if (!existed)
                {
                    user = new User
                    {
                        Username = username,
                        Email = email,
                        Password = hashedPassword,
                        RoleId = 2,
                        Status = "Pending"
                    };

                    _userService.Add(user);
                    break;
                }
            }

            var loginUrl = Url.Action("Login", "Auth", new
            {
                username = user.Username,
                password = rawPassword
            }, Request.Scheme);

            TempData["Message"] = $"✅ Đã tạo Recruiter: <b>{username}</b> / <b>{rawPassword}</b>";
            TempData["RecruiterLoginLink"] = loginUrl;

            return RedirectToAction("UserList");
        }

    }
}
