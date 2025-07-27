using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Jobs;
using Service.Interface;
using TopCVWeb.Models;
using Service.Recruiters;
using Service.Cvs;

namespace TopCVWeb.Controllers
{
    public class JobController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IJobService _jobService;
        private readonly IJobSeekerService _jobSekkerService;
        private readonly IJobsService _jobsService;
        private readonly IRecruiterService _recruiterService;
        private readonly ICategoryService _categoryService;
        private readonly ICvService _cvService;

        public JobController(ILogger<HomeController> logger, IJobService jobService, IRecruiterService recruiterService, IJobSeekerService jobSekkerService, IJobsService jobsService)
        {
            _logger = logger;
            _jobService = jobService;
            _jobSekkerService = jobSekkerService;
            _jobsService = jobsService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            _logger.LogInformation("ok1");
            var seekerId = await GetCurrentSeekerIdAsync();
            _logger.LogInformation("ok2");
            var viewModel = await _jobService.GetJobDetailViewModelAsync(id, seekerId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        public async Task<IActionResult> SavedJobs()
        {
            var seekerId = await GetCurrentSeekerIdAsync();
            if (!seekerId.HasValue)
            {
                return RedirectToAction("Login", "Auth");
            }

            var savedJobs = await _jobService.GetSavedJobsAsync(seekerId.Value);
            return View(savedJobs);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleSave(int jobId)
        {
            var seekerId = await GetCurrentSeekerIdAsync();
            if (!seekerId.HasValue)
            {
                return Json(new { success = false, redirectUrl = Url.Action("Login", "Auth") });
            }

            try
            {
                var isSaved = await _jobService.ToggleSaveJobAsync(seekerId.Value, jobId);
                return Json(new { success = true, saved = isSaved });
            }
            catch
            {
                return Json(new { success = false, message = "Đã có lỗi xảy ra." });
            }
        }
        private async Task<int?> GetCurrentSeekerIdAsync()
        {
            var userIdString = HttpContext.Session.GetInt32("userId");
            return await _jobSekkerService.GetSeekerIdFromUserIdAsync(userIdString);
        }
        public IActionResult MyJobs()
        {
            // int? userId = HttpContext.Session.GetInt32("UserId");
            // if (userId == null)
            //   return RedirectToAction("Login", "Account");
            int userId = 1;
            //  var jobs = _jobsService.GetJobsByUserId(userId.Value);

            var recruiter = _recruiterService.GetByUserId(userId);
            if (recruiter == null)
                return Content("Không tìm thấy recruiter tương ứng với tài khoản.");

            var jobs = _jobsService.GetJobsByUserId(userId);
            return View(jobs);
        }

        public IActionResult ManageApplicants()
        {
            int userId = 1; // test cứng
            var recruiter = _recruiterService.GetByUserId(userId);
            if (recruiter == null)
                return Content("Không tìm thấy recruiter");

            var jobs = _jobsService.GetJobsWithApplicantsCountByRecruiterId(recruiter.RecruiterId);

            // Chuyển sang ViewModel để đưa ra View
            var viewModels = jobs.Select(j => new JobWithApplicantsCountViewModel
            {
                JobId = j.JobId,
                JobTitle = j.JobTitle,
                ApplicantCount = j.Applications?.Count ?? 0
            }).ToList();

            return View(viewModels);
        }

        public IActionResult ViewCv(int cvId)
        {
            var cv = _cvService.GetById(cvId);
            if (cv == null || cv.CvLink == null)
                return Content("Không tìm thấy CV.");

            return File(cv.CvLink, "application/pdf", $"cv_{cvId}.pdf");
        }

        public IActionResult Create()
        {
            var categories = _categoryService.GetAll();

            // Gửi sang View bằng ViewBag hoặc ViewData
            ViewBag.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Create(Job job)
        {
            // Lấy UserId từ session
            //  int? userId = HttpContext.Session.GetInt32("UserId");
            //   if (userId == null)
            //     return RedirectToAction("Login", "Account");
            int userId = 1;
            // Validate recruiter
            var recruiter = _recruiterService.GetByUserId(userId);
            if (recruiter == null)
            {
                TempData["ErrorMessage"] = "Tài khoản của bạn chưa đăng ký thông tin nhà tuyển dụng.";
                return RedirectToAction("MyJobs");
            }

            // Nếu CreateDate không nhập từ form → set mặc định hôm nay
            job.CreateDate = DateOnly.FromDateTime(DateTime.Now);

            // Kiểm tra ngày kết thúc
            if (job.EndDate < job.CreateDate)
            {
                ModelState.AddModelError("EndDate", "Ngày kết thúc không được nhỏ hơn ngày tạo.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    job.RecruiterId = recruiter.RecruiterId;
                    _jobsService.Create(job);

                    TempData["SuccessMessage"] = "Đăng bài thành công!";
                    return RedirectToAction("MyJobs");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }

            return View(job);
        }

        public IActionResult Edit(int id)
        {
            var job = _jobsService.GetById(id);
            if (job == null)
            {
                return NotFound("Không tìm thấy bài tuyển dụng.");
            }

            ViewBag.Categories = _categoryService.GetAll()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            return View(job);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Job job)
        {
            if (!ModelState.IsValid)
            {
                // Nếu dữ liệu không hợp lệ, trả lại view cùng danh sách Category
                ViewBag.Categories = _categoryService.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                return View(job);
            }

            // Lấy dữ liệu cũ để giữ lại các trường không cho phép chỉnh sửa
            var existingJob = _jobsService.GetById(job.JobId);
            if (existingJob == null)
            {
                return NotFound("Không tìm thấy bài tuyển dụng.");
            }

            try
            {
                // ✅ Giữ lại RecruiterId & CreateDate
                job.RecruiterId = existingJob.RecruiterId;
                job.CreateDate = existingJob.CreateDate;

                // ✅ Cập nhật DB
                _jobsService.Update(job);

                TempData["SuccessMessage"] = "Cập nhật bài tuyển dụng thành công!";
                return RedirectToAction("MyJobs");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Có lỗi khi cập nhật: {ex.Message}");

                ViewBag.Categories = _categoryService.GetAll()
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoryId.ToString(),
                        Text = c.CategoryName
                    }).ToList();

                return View(job);
            }
        }

        // POST: Xác nhận xóa job
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _jobsService.Delete(id);
            return RedirectToAction("MyJobs");
        }

    }
}
