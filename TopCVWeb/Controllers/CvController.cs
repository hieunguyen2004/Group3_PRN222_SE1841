using Microsoft.AspNetCore.Mvc;
using Service.Cvs;

namespace TopCVWeb.Controllers
{
    public class CvController : Controller
    {
        private readonly ICvService _cvService;

        public CvController(ICvService cvService)
        {
            _cvService = cvService;
        }

        /// <summary>
        /// API cập nhật trạng thái CV (Nộp đơn / Chấp nhận / Từ chối)
        /// </summary>
        [HttpPost]
        public IActionResult UpdateStatus([FromBody] UpdateStatusRequest req)
        {
            try
            {
                if (req == null || req.Id <= 0)
                {
                    return Json(new { success = false, message = "Dữ liệu không hợp lệ." });
                }

                var cv = _cvService.GetById(req.Id);
                if (cv == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy CV." });
                }

                cv.CvStatus = req.Status;
                _cvService.Update(cv);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }

    /// <summary>
    /// Request model nhận từ Fetch (JS)
    /// </summary>
    public class UpdateStatusRequest
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
