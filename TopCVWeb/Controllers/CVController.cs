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
        public async Task<IActionResult> List()
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return Unauthorized(); // or RedirectToAction("Login")
            }

            var seeker = await _jobSeekerService.GetJobSeekerByUser(userId.Value);
            if (seeker == null)
            {
                return Unauthorized();
            }

            var cvs = await _cvService.GetCVsBySeekerIdAsync(seeker.SeekerId);

            return View(cvs);
        }


        [HttpPost]
        public async Task<IActionResult> Upload(int seekerId, IFormFile cvFile)
        {
            int? userId = HttpContext.Session.GetInt32("userId");
            if (userId == null)
            {
                return Unauthorized(); // or RedirectToAction("Login")
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
                await cvFile.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            //bool exists = await _cvService.ExistsByContentAsync(fileBytes);
            //if (exists)
            //{
            //    TempData["Error"] = "This CV file already exists.";
            //    return RedirectToAction("UploadForm");
            //}

            var seeker = await _jobSeekerService.GetJobSeekerByUser(userId.Value);
            var cv = new Cv
            {
                SeekerId =  seeker.SeekerId,
                CvStatus = "Pending",
                CvLink = fileBytes,
                FileName = Path.GetFileName(cvFile.FileName)
            };

            await _cvService.AddCVAsync(cv);
            await _applicationService.AddApplication(8, cv.CvId);
            

            TempData["Success"] = "CV uploaded and application submitted!";
            return RedirectToAction("List");
        }



        public async Task<IActionResult> ViewCV(int cvId)
        {
            var cv = await _cvService.GetCVByIdAsync(cvId);
            if (cv == null || cv.CvLink == null)
            {
                return NotFound();
            }

            return File(cv.CvLink, "application/pdf");

        }


    }

}
