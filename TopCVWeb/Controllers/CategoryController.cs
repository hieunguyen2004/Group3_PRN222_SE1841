using DAO.Models;
using Microsoft.AspNetCore.Mvc;
using Service.CategoryServices;

namespace TopCVWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public IActionResult Add([FromBody] Category category)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return Json(new { success = false, message = "Tên danh mục không được để trống." });
            }

            try
            {
                // Thêm mới category
                _categoryService.Add(category);

                return Json(new
                {
                    success = true,
                    categoryId = category.CategoryId,   // EF sẽ tự cập nhật sau khi SaveChanges
                    categoryName = category.CategoryName
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
