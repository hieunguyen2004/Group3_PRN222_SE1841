using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DAO.Models;

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
        public IActionResult UserList(string? searchTerm, int page = 1, int pageSize = 10)
        {
            try
            {
                var users = _userService.GetAll();

                // Tìm kiếm theo Username hoặc Email
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    users = users
                        .Where(u =>
                            (!string.IsNullOrEmpty(u.Username) && u.Username.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                            (!string.IsNullOrEmpty(u.Email) && u.Email.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
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
    }
}
