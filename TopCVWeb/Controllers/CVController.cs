using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class CVController : Controller
    {
        private readonly ICVService _cvService;
        private readonly IWebHostEnvironment _env;

        public CVController(ICVService cvService, IWebHostEnvironment env)
        {
            _cvService = cvService;
            _env = env;
        }
        public async Task<IActionResult> List()
        {
            //int userId = 2; // From session or auth

            //var seeker = await _context.JobSeekers.FirstOrDefaultAsync(js => js.UserId == userId);
            //if (seeker == null)
            //{
            //    return Unauthorized();
            //}

            var cvs = await _cvService.GetCVsBySeekerIdAsync(2);
            ViewBag.SeekerId = 2;

            return View(cvs);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int seekerId, IFormFile cvFile)
        {
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

            
            bool exists = await _cvService.ExistsByContentAsync(fileBytes);
            if (exists)
            {
                TempData["Error"] = "This CV file already exists.";
                return RedirectToAction("UploadForm");
            }

            var cv = new Cv
            {
                SeekerId = 2,
                CvStatus = "Pending",
                CvLink = fileBytes
            };

            await _cvService.AddCVAsync(cv);

            TempData["Success"] = "CV uploaded and saved to DB!";
            return RedirectToAction("Upload");
        }


        public async Task<IActionResult> ViewCV(int cvId)
        {
            var cv = await _cvService.GetCVByIdAsync(cvId);
            if (cv == null || cv.CvLink == null)
            {
                return NotFound();
            }

            return File(cv.CvLink, "application/pdf", "cv.pdf");
        }
        

    }

}
