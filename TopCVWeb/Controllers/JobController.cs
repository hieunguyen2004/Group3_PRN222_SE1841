using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace TopCVWeb.Controllers
{
    public class JobController : Controller
    {

        private readonly ILogger<HomeController> _logger;
        private readonly IJobService _jobService;
        private readonly IJobSeekerService _jobSekkerService;

        public JobController(ILogger<HomeController> logger, IJobService jobService, IJobSeekerService jobSekkerService)
        {
            _logger = logger;
            _jobService = jobService;
            _jobSekkerService = jobSekkerService;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var seekerId = await GetCurrentSeekerIdAsync();
            var viewModel = await _jobService.GetJobDetailViewModelAsync(id, seekerId);

            if (viewModel == null)
            {
                return NotFound();
            }

            int check = await _jobService.UpdateJobNumberOfSeeker(id);
            _logger.LogInformation($"Job detail viewed: {id} by Seeker: {seekerId}");
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
    }
}
