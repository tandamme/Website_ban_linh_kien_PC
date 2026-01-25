using Do_an_lap_trinh_c_.Data;
using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LinhkienpcContext _context;

        public HomeController(ILogger<HomeController> logger, LinhkienpcContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Home loads top products and passes them to the view
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsNoTracking()
                .OrderByDescending(p => p.SanPhamMoi)
                .ThenByDescending(p => p.SoLanXem)
                .Take(8)
                .ToListAsync();

            return View("~/Views/Home/home.cshtml", products);
        }


        // AJAX search endpoint: searches by product name OR category name
        [HttpGet("/api/products/search")]
        public async Task<IActionResult> SearchProducts(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return Json(new object[0]);

            var q = term.Trim().ToLowerInvariant();

            var initialQuery = _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsNoTracking()
                .Where(p =>
                    EF.Functions.Like((p.TenSanPham ?? string.Empty).ToLower(), $"%{q}%")
                    || EF.Functions.Like((p.MaLoaiNavigation != null ? (p.MaLoaiNavigation.TenLoai ?? string.Empty) : string.Empty).ToLower(), $"%{q}%")
                );

            var candidates = await initialQuery.Take(1000).ToListAsync();

            // Tokenize and normalize the search term for in-memory filtering
            var normTokens = Normalize(term).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Final in-memory filter: require all tokens appear in name OR category (accent-insensitive)
            var matched = candidates
                .Where(p =>
                {
                    var name = Normalize(p.TenSanPham);
                    var category = Normalize(p.MaLoaiNavigation?.TenLoai);
                    return normTokens.All(tok => (!string.IsNullOrEmpty(name) && name.Contains(tok)) || (!string.IsNullOrEmpty(category) && category.Contains(tok)));
                })
                .OrderByDescending(p => p.SanPhamMoi)
                .ThenByDescending(p => p.SoLanXem)
                .Take(50)
                .ToList();

            var resultList = matched.Select(p => new
            {
                id = p.MaSanPham,
                name = p.TenSanPham,
                price = p.DonGia,
                image = string.IsNullOrEmpty(p.Hinh)
                    ? Url.Content("~/img/no-image.png")
                    : (p.Hinh!.StartsWith("~") ? Url.Content(p.Hinh) : Url.Content($"~/images/san_pham/{p.Hinh}")),
                category = p.MaLoaiNavigation != null ? p.MaLoaiNavigation.TenLoai : null,
                detailUrl = Url.Action("ProductDetail", "Home", new { id = p.MaSanPham })
            }).ToList();

            return Json(resultList);
        }

        // Load products list view (server-side)
        public async Task<IActionResult> Product(string? category, string? search)
        {
            var query = _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                var catLower = category.ToLowerInvariant();
                query = query.Where(p =>
                    (p.MaLoaiNavigation != null && (p.MaLoaiNavigation.TenLoai ?? string.Empty).ToLower().Replace(" ", "-") == catLower)
                );
            }

            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLowerInvariant();
                query = query.Where(p => (p.TenSanPham ?? string.Empty).ToLower().Contains(s));
            }

            var products = await query.ToListAsync();
            return View("~/Views/Home/product.cshtml", products);
        }

        // Show product details by id
        public async Task<IActionResult> ProductDetail(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MaSanPham == id.Value);

            if (product == null)
                return NotFound();

            return View("~/Views/Home/ProductDetail.cshtml", product);
        }

        public IActionResult Notification() => View("~/Views/Home/notification.cshtml");
        public IActionResult AboutUs() => View("~/Views/Home/aboutUs.cshtml");
        public IActionResult ContactUs() => View("~/Views/Home/contactUs.cshtml");
        public IActionResult Cart() => View("~/Views/Home/cart.cshtml");
        public IActionResult CheckOut() => View("~/Views/Home/checkOut.cshtml");
        public IActionResult AddProduct() => View("~/Views/Admin/addProduct.cshtml");
        public IActionResult EditProduct() => View("~/Views/Admin/editProduct.cshtml");
        public IActionResult ProductManagement(int? loai)
        {
            var data = _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsNoTracking()
                .AsQueryable();

            if (loai.HasValue)
                data = data.Where(p => p.MaLoai == loai);

            ViewBag.Loai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai"
            );

            return View("~/Views/Admin/productManagement.cshtml", data.ToList());
        }

        public IActionResult CategoryManagement() => View("~/Views/Admin/categoryManagement.cshtml");
        public IActionResult UserManagement() => View("~/Views/Admin/userManagement.cshtml");
        public IActionResult Login() => View("~/Views/Admin/login.cshtml");
        public IActionResult Signup() => View("~/Views/Admin/signup.cshtml");

        [HttpPost]
        public IActionResult ContactSuccess(string name, string email, string message)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            return View();
        }

        public IActionResult CheckOutSuccess() => View("~/Views/Home/checkOutSuccess.cshtml");

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Add this helper method inside the HomeController class
        private static string Normalize(string? input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            // Remove diacritics (accents), convert to lower case, and trim
            var normalized = input.Normalize(System.Text.NormalizationForm.FormD);
            var chars = normalized.Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).ToLowerInvariant().Trim();
        }
    }
}
