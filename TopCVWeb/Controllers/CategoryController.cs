using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using DAO.Models;
using Microsoft.EntityFrameworkCore;

public class CategoryController : Controller
{
    private readonly ICategoryService _service;
    private const int PageSize = 10;

    public CategoryController(ICategoryService service)
    {
        _service = service;
    }

    public async Task<IActionResult> Index(string? searchTerm, int page = 1)
    {
        var categories = await _service.GetAllAsync();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            categories = categories
                .Where(c => c.CategoryName != null && c.CategoryName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        int totalItems = categories.Count;
        int totalPages = (int)Math.Ceiling((double)totalItems / PageSize);

        var pagedCategories = categories
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.SearchTerm = searchTerm;

        return View(pagedCategories);
    }

    public async Task<IActionResult> Details(int id)
    {
        var category = await _service.GetByIdAsync(id);
        return category == null ? NotFound() : View(category);
    }

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        await _service.AddAsync(category);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var category = await _service.GetByIdAsync(id);
        return category == null ? NotFound() : View(category);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Category category)
    {
        if (!ModelState.IsValid) return View(category);
        await _service.UpdateAsync(category);
        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
        }
        catch (DbUpdateException)
        {
            TempData["ErrorMessage"] = "❌ Không thể xoá vì danh mục đang được sử dụng trong công việc.";
        }
        return RedirectToAction("Index");
    }
}
