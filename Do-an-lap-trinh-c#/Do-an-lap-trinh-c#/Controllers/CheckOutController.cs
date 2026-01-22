using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class CheckOutController : Controller
    {
        // Trang cart.cshtml (giả lập dữ liệu tĩnh)
        public IActionResult Cart { get; }


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
