using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System.Diagnostics;
using TopCVWeb.Models;

namespace TopCVWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJobService _jobService;
        private readonly IJobSeekerService _jobSekkerService;

        public HomeController(ILogger<HomeController> logger, IJobService jobService, IJobSeekerService jobSekkerService)
        {
            _logger = logger;
            _jobService = jobService;
            _jobSekkerService = jobSekkerService;
        }

        public async Task<IActionResult> Index(string? searchTitle, int? categoryId, int? companyId, int pageIndex = 1, int pageSize = 3)
        {
            var seekerId = await GetCurrentSeekerIdAsync();
            var viewModel = await _jobService.GetHomeViewModelAsync(searchTitle, categoryId, companyId, pageIndex, pageSize, seekerId);
            return View(viewModel);
        }

        private async Task<int?> GetCurrentSeekerIdAsync()
        {
            var userIdString = HttpContext.Session.GetInt32("userId");
            return await _jobSekkerService.GetSeekerIdFromUserIdAsync(userIdString);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
