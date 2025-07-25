using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace WebApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _companyService;

        public CompanyController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        // GET: /Company
        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAllAsync();
            return View(companies);
        }

        // GET: /Company/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyService.CreateAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: /Company/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: /Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Company company)
        {
            if (ModelState.IsValid)
            {
                await _companyService.UpdateAsync(company);
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: /Company/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var company = await _companyService.GetByIdAsync(id);
            if (company == null) return NotFound();
            return View(company);
        }

        [HttpGet("Company/Search")]
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var companies = await _companyService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                companies = companies
                    .Where(c => c.CompanyName != null && c.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(companies);
        }

    }
}
