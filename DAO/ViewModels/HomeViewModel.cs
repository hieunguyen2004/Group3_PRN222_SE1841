using DAO.Models;
using TopCVWeb.Util;

namespace TopCVWeb.ViewModels
{
    public class HomeViewModel
    {
        public PaginatedList<JobViewModel> Jobs { get; set; }
        public List<Category> Categories { get; set; }
        public List<Company> Companies { get; set; }

        // Dùng để giữ lại giá trị filter trên View
        public string? SearchTitle { get; set; }
        public int? CategoryId { get; set; }
        public int? CompanyId { get; set; }
        public int PageSize { get; set; }
    }
}
