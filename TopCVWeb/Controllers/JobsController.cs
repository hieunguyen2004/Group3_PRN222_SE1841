using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;
using Service.Cvs;
using Service.Jobs;
using Service.Recruiters;
using TopCVWeb.Models;

namespace TopCVWeb.Controllers
{
    public class JobsController : Controller
    {
        private readonly IJobsService _jobsService;
        private readonly IRecruiterService _recruiterService;
        private readonly IApplicationService _applicationService;
        private readonly ICvService _cvService;
        public JobsController(IJobsService jobsService, IRecruiterService recruiterService, IApplicationService applicationService, ICvService cvService)
        {
            _jobsService = jobsService;
            _recruiterService = recruiterService;
            _applicationService = applicationService;
            _cvService = cvService;
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
                    Status = a.Status
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
        public IActionResult Create()
        {
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

                _jobsService.Create(job);
                return RedirectToAction("MyJobs");
            }

            return View(job);
        }


        // GET: Hiển thị form chỉnh sửa
        public IActionResult Edit(int id)
        {
            var job = _jobsService.GetById(id);
            if (job == null)
                return NotFound();

            return View(job);
        }

        // POST: Cập nhật job
        [HttpPost]
        public IActionResult Edit(Job job)
        {
            if (ModelState.IsValid)
            {
                // Lấy job cũ từ DB để giữ lại RecruiterId
                var oldJob = _jobsService.GetById(job.JobId);
                if (oldJob == null)
                    return NotFound();

                job.RecruiterId = oldJob.RecruiterId; 
                job.CreateDate = oldJob.CreateDate;   

                _jobsService.Update(job);
                return RedirectToAction("MyJobs");
            }
            return View(job);
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
