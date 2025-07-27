using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class CVController : Controller
    {
        private readonly ICVService _cvService;
        private readonly IApplicationService _applicationService;
        private readonly IJobSeekerService _jobSeekerService;
        private readonly IWebHostEnvironment _env;

        public CVController(ICVService cvService, IWebHostEnvironment env,IApplicationService applicationService, IJobSeekerService jobSeekerService)
        {
            _cvService = cvService;
            _env = env;
            _applicationService = applicationService;
            _jobSeekerService = jobSeekerService;
        }
        public IActionResult List()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var seeker =   _jobSeekerService.GetJobSeekerByUser(userId.Value);
            if (seeker == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cvs =   _cvService.GetCVsBySeekerId(seeker.SeekerId);

            return View(cvs);
        }


        [HttpPost]
        public IActionResult Upload(int seekerId, IFormFile cvFile, int jobId)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }
            if (cvFile == null || cvFile.Length == 0)
            {
                TempData["Error"] = "Please upload a valid PDF file.";
                return RedirectToAction("UploadForm");
            }

            var ext = Path.GetExtension(cvFile.FileName).ToLower();
            if (ext != ".pdf")
            {
                TempData["Error"] = "Only PDF files are allowed.";
                return RedirectToAction("UploadForm");
            }

            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                 cvFile.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            

            var seeker =   _jobSeekerService.GetJobSeekerByUser(userId.Value);
            var cv = new Cv
            {
                SeekerId =  seeker.SeekerId,
                CvStatus = "Pending",
                CvLink = fileBytes,
                FileName = Path.GetFileName(cvFile.FileName)
            };

              _cvService.AddCV(cv);
              _applicationService.AddApplication(jobId, cv.CvId);
            

            TempData["Success"] = "CV uploaded and application submitted!";
            return RedirectToAction("List");
        }



        public IActionResult ViewCV(int cvId)
        {
            var cv =   _cvService.GetCVById(cvId);
            if (cv == null || cv.CvLink == null)
            {
                return NotFound();
            }

            return File(cv.CvLink, "application/pdf");

        }


    }

}
