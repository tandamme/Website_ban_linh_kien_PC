using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class CheckOutController : Controller
    {
        // Trang cart.cshtml (giả lập dữ liệu tĩnh)
        public IActionResult Cart()
        {
            var cartItems = new List<CartItem>
            {
                new CartItem { Id=1, Image="~/img/linh-kien-roi/gpu1.png", Name="GPU RTX 4060", Price=7500000, Quantity=1 },
                new CartItem { Id=2, Image="~/img/combo-pc/sinh-vien.jpg", Name="Combo PC Sinh viên", Price=12500000, Quantity=2 },
            };

            return View(cartItems);
        }

        // Nhận dữ liệu từ form POST và chuyển sang checkout
        [HttpPost]
        public IActionResult SaveCart(string cartData)
        {
            if (string.IsNullOrEmpty(cartData))
                return RedirectToAction("Cart");

            // Deserialize JSON ra list
            var cartItems = JsonConvert.DeserializeObject<List<CartItem>>(cartData);

            // Chuyển sang view checkout
            return View("~/Views/Home/checkOut.cshtml", cartItems);
        }

        // Trang checkout.cshtml có thể trực tiếp nhận List<CartItem>
        public IActionResult CheckOut(List<CartItem> cartItems)
        {
            return View("~/Views/Home/checkOut.cshtml", cartItems);
        }
    }
}
