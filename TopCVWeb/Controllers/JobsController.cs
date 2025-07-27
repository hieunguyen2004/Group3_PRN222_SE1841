using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Cvs;
using Service.Jobs;
using Service.Recruiters;
using TopCVWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TopCVWeb.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobsService _jobsService;
        private readonly IRecruiterService _recruiterService;
        private readonly IApplicationService _applicationService;
        private readonly ICvService _cvService;
        private readonly ICVService _cv2Service;
        private readonly ICategoryService _categoryService;
        public JobsController(IJobsService jobsService, IRecruiterService recruiterService, IApplicationService applicationService, ICvService cvService, ICVService cv2Service, ICategoryService categoryService)
        {
            _jobsService = jobsService;
            _recruiterService = recruiterService;
            _applicationService = applicationService;
            _cvService = cvService;
            _cv2Service = cv2Service;
            _categoryService = categoryService;
        }



        // Hiển thị danh sách job theo recruiter hiện tại
        public IActionResult MyJobs()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");
           
            

            var recruiter = _recruiterService.GetByUserId(userId);
            if (recruiter == null)
                return Content("Không tìm thấy recruiter tương ứng với tài khoản.");

            var jobs = _jobsService.GetJobsByUserId(userId);
            return View(jobs);
        }

        // Trang thống kê ứng viên theo từng bài
        public IActionResult ManageApplicants()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
                return RedirectToAction("Login", "Auth");
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

        public IActionResult Applicants(int jobId)
        {
            var applicants = _applicationService.GetApplicantsByJobId(jobId)
                .Select(a => new ApplicantViewModel
                {
                    ApplicationId = a.ApplicationId,
                    CvId = a.CvId ?? 0,
                    FullName = a.Cv.Seeker.User.Lastname,
                    SubmitDate = a.SubmitDate,
                    Status = a.Cv.CvStatus,
                    Email = a.Cv?.Seeker?.User?.Email
                }).ToList();

            ViewBag.JobId = jobId;
            return View(applicants);
        }

        public IActionResult ViewCv(int cvId)
        {
            var cv = _cvService.GetById(cvId);
            if (cv == null || cv.CvLink == null)
                return Content("Không tìm thấy CV.");

            return File(cv.CvLink, "application/pdf", $"cv_{cvId}.pdf");
        }

        // GET: Hiển thị form tạo mới
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();

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
            if (ModelState.IsValid)
            {


                int? userId = HttpContext.Session.GetInt32("userId");
                if (userId == null)
                    return RedirectToAction("Login", "Auth");


                // Lấy recruiter tương ứng với userId
                var recruiter = _recruiterService.GetByUserId(userId);
                if (recruiter == null)
                    return Content("Không tìm thấy recruiter tương ứng.");

                job.RecruiterId = recruiter.RecruiterId;
                job.CreateDate = DateOnly.FromDateTime(DateTime.Now);

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
                _jobsService.Create(job);
                return RedirectToAction("MyJobs");
            }

            return View(job);
        }


        // GET: Hiển thị form chỉnh sửa
        public async Task<IActionResult> Edit(int id)
        {
            var job = _jobsService.GetById(id);
            if (job == null)
                return NotFound();

            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories
               .Select(c => new SelectListItem
               {
                   Value = c.CategoryId.ToString(),
                   Text = c.CategoryName
               }).ToList();

            return View(job);
        }

        // POST: Cập nhật job
        [HttpPost]
        public async Task<IActionResult>  Edit(Job job)
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

        [HttpGet]
        public IActionResult ConfirmCV(int cvId, string email, int appId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["Error"] = "Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Auth");
            }

            _cv2Service.ConfirmCv(cvId, email,appId,userId.Value); 

            TempData["Message"] = "CV has been confirmed as applied.";
            return Redirect(Request.Headers["Referer"].ToString());
        }


        [HttpGet]
        public IActionResult RejectCv(int cvId, string email, int appId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                TempData["Error"] = "Phiên đăng nhập đã hết. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Auth");
            }

            _cv2Service.RejectCv(cvId, email, appId, userId.Value);

            TempData["Message"] = "CV has been confirmed as applied.";
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
