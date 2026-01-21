using System.Diagnostics;
using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Home()
        {
            return View("~/Views/Home/home.cshtml");
        }

        public IActionResult Product()
        {
            return View("~/Views/Home/product.cshtml");
        }

        public IActionResult ProductDetail()
        {
            return View("~/Views/Home/ProductDetail.cshtml");
        }

        public IActionResult Notification()
        {
            return View("~/Views/Home/notification.cshtml");
        }

        public IActionResult AboutUs()
        {
            return View("~/Views/Home/aboutUs.cshtml");
        }

        public IActionResult ContactUs()
        {
            return View("~/Views/Home/contactUs.cshtml");
        }

        public IActionResult Cart()
        {
            return View("~/Views/Home/cart.cshtml");
        }

        public IActionResult CheckOut()
        {
            return View("~/Views/Home/checkOut.cshtml");
        }

        public IActionResult AddProduct()
        {
            return View("~/Views/Admin/addProduct.cshtml");
        }

        public IActionResult EditProduct()
        {
            return View("~/Views/Admin/editProduct.cshtml");
        }

        public IActionResult ProductManagement()
        {
            return View("~/Views/Admin/productManagement.cshtml");
        }

        public IActionResult CategoryManagement()
        {
            return View("~/Views/Admin/categoryManagement.cshtml");
        }

        public IActionResult UserManagement()
        {
            return View("~/Views/Admin/userManagement.cshtml");
        }

        public IActionResult Login()
        {
            return View("~/Views/Admin/login.cshtml");
        }

        public IActionResult Signup()
        {
            return View("~/Views/Admin/signup.cshtml");
        }

        [HttpPost]
        public IActionResult ContactSuccess(string name, string email, string message)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            return View();
        }

        public IActionResult CheckOutSuccess()
        {
            return View("~/Views/Home/checkOutSuccess.cshtml");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
