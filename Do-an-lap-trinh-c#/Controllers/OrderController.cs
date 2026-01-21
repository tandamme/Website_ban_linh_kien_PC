using Microsoft.AspNetCore.Mvc;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class OrderController : Controller
    {
        [HttpPost]
        public IActionResult PlaceOrder(string FullName, string Email, string Phone, string Address, string PaymentMethod)
        {
            // Ở đây bạn có thể lưu đơn hàng vào DB nếu muốn
            // Hiện tại chúng ta giả lập, chỉ cần chuyển sang trang success

            // Lưu thông tin khách hàng vào TempData
            TempData["FullName"] = FullName;
            TempData["Email"] = Email;
            TempData["PaymentMethod"] = PaymentMethod;

            // Redirect đến view success
            return RedirectToAction("CheckOutSuccess", "Home");
        }
    }
}
