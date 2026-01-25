using Do_an_lap_trinh_c_.Data;
using Do_an_lap_trinh_c_.Extensions;
using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class CartController : Controller
    {
        private const string CartSessionKey = "Cart.Session";
        private readonly LinhkienpcContext _context;

        public CartController(LinhkienpcContext context)
        {
            _context = context;
        }

        public List<CartItem> GetCart()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey);
            if (cart == null)
            {
                cart = new List<CartItem>();
                HttpContext.Session.SetObject(CartSessionKey, cart);
            }
            return cart;
        }
        public IActionResult Index() {
            var cart = GetCart();
            return View(cart);
        }
        public IActionResult AddtoCart(int id ,int quanlity =1)
        {
            var product = _context.Products.AsNoTracking().FirstOrDefault(p => p.MaSanPham == id);
            if (product == null) return NotFound();
            var cart = GetCart();
            var existing = cart.FirstOrDefault(c => c.ProductId == id);
            if (existing != null)
            {
                existing.Quantity += 1;
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.MaSanPham,
                    Name = product.TenSanPham,
                    Quantity = 1,
                    Price = product.DonGia,
                    Image = string.IsNullOrEmpty(product.Hinh) ? null : (product.Hinh!.StartsWith("~") ? Url.Content(product.Hinh) : $"/images/san_pham/{product.Hinh}")
                });
            }
            HttpContext.Session.SetObject(CartSessionKey, cart);
            return RedirectToAction("Index");
        }
        //// POST /cart/add
        //[HttpPost("/cart/add")]
        //[IgnoreAntiforgeryToken]
        //public async Task<IActionResult> Add([FromBody] AddRequest req)
        //{
        //    if (req is null || req.ProductId <= 0) return BadRequest(new { ok = false, message = "Invalid request" });

        //    var product = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.MaSanPham == req.ProductId);
        //    if (product == null) return NotFound(new { ok = false, message = "Product not found" });

        //    var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();

        //    var existing = cart.FirstOrDefault(c => c.ProductId == req.ProductId);
        //    if (existing != null)
        //    {
        //        existing.Quantity += Math.Max(1, req.Quantity);
        //    }
        //    else
        //    {
        //        cart.Add(new CartItem
        //        {
        //            ProductId = product.MaSanPham,
        //            Name = product.TenSanPham,
        //            Quantity = Math.Max(1, req.Quantity),
        //            Price = product.DonGia,
        //            Image = string.IsNullOrEmpty(product.Hinh) ? null : (product.Hinh!.StartsWith("~") ? Url.Content(product.Hinh) : $"/images/san_pham/{product.Hinh}")
        //        });
        //    }

        //    HttpContext.Session.SetObject(CartSessionKey, cart);

        //    var totalItems = cart.Sum(x => x.Quantity);
        //    return Json(new { ok = true, totalItems });
        //}

        // GET /cart/items

        // POST /cart/add
        [HttpPost("/cart/add")]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> Add([FromBody] AddRequest req)
        {
            if (req == null || req.ProductId <= 0)
                return BadRequest(new { ok = false });

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MaSanPham == req.ProductId);

            if (product == null)
                return NotFound(new { ok = false });

            var cart = GetCart();

            var existing = cart.FirstOrDefault(x => x.ProductId == req.ProductId);

            if (existing != null)
            {
                existing.Quantity += req.Quantity;   // ⭐ TĂNG Ở ĐÂY
            }
            else
            {
                cart.Add(new CartItem
                {
                    ProductId = product.MaSanPham,
                    Name = product.TenSanPham,
                    Quantity = req.Quantity,
                    Price = product.DonGia,
                    Image = string.IsNullOrEmpty(product.Hinh)
                            ? null
                            : (product.Hinh!.StartsWith("~")
                                ? Url.Content(product.Hinh)
                                : $"/images/san_pham/{product.Hinh}")
                });
            }

            HttpContext.Session.SetObject(CartSessionKey, cart);

            return Json(new
            {
                ok = true,
                quantity = existing?.Quantity ?? req.Quantity
            });
        }

        [HttpGet("/cart/items")]
        public IActionResult Items()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            return Json(cart);
        }

        // POST /cart/remove
        [HttpPost("/cart/remove")]
        [IgnoreAntiforgeryToken]
        public IActionResult Remove([FromBody] RemoveRequest req)
        {
            if (req is null || req.ProductId <= 0) return BadRequest(new { ok = false });

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CartSessionKey) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(c => c.ProductId == req.ProductId);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObject(CartSessionKey, cart);
            }

            return Json(new { ok = true, totalItems = cart.Sum(x => x.Quantity) });
        }

        public class AddRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; } = 1;
        }

        public class RemoveRequest
        {
            public int ProductId { get; set; }
        }
    }
}