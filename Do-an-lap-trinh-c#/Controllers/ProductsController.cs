using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_lap_trinh_c_.Data;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class ProductsController : Controller
    {
        private readonly LinhkienpcContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(LinhkienpcContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // ================== LIST (ADMIN) ==================
        public IActionResult Index(int? loai)
        {
            var query = _context.Products
                .Include(p => p.MaLoaiNavigation)
                .AsQueryable();

            if (loai.HasValue)
            {
                query = query.Where(p => p.MaLoai == loai);
            }

            ViewBag.Loai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai"
            );

            return View(query.ToList());
        }

        // ================== DETAILS ==================
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products
                .Include(p => p.MaLoaiNavigation)
                .FirstOrDefaultAsync(p => p.MaSanPham == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // ================== CREATE ==================
        // GET: Products/AddProduct
        [HttpGet]
        public IActionResult AddProduct()
        {
            ViewBag.MaLoai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai"
            );

            return View("~/Views/Products/addProduct.cshtml");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? ImageFile)
        {
            product.SoLanXem = 0;
            product.NgayTao = DateOnly.FromDateTime(DateTime.Now);

            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "images/products");
                Directory.CreateDirectory(folder);

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folder, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);

                product.Hinh = fileName;
            }
            else
            {
                product.Hinh = "no-image.png";
            }

            if (!ModelState.IsValid)
            {
                ViewBag.MaLoai = new SelectList(
                    _context.Typeproducts,
                    "MaLoai",
                    "TenLoai",
                    product.MaLoai
                );
                return View("~/Views/Products/addProduct.cshtml", product);
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProductManagement", "Home");
        }

        // ================== EDIT ==================
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.MaLoai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai",
                product.MaLoai
            );

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
        {
            if (id != product.MaSanPham) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.MaLoai = new SelectList(
                    _context.Typeproducts,
                    "MaLoai",
                    "TenLoai",
                    product.MaLoai
                );
                return View(product);
            }

            var oldProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.MaSanPham == id);

            if (oldProduct == null) return NotFound();

            // ===== Image update =====
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folder = Path.Combine(_env.WebRootPath, "images/products");

                if (!string.IsNullOrEmpty(oldProduct.Hinh))
                {
                    string oldPath = Path.Combine(folder, oldProduct.Hinh);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string newPath = Path.Combine(folder, fileName);

                using var stream = new FileStream(newPath, FileMode.Create);
                await ImageFile.CopyToAsync(stream);

                product.Hinh = fileName;
            }
            else
            {
                product.Hinh = oldProduct.Hinh;
            }

            product.SoLanXem = oldProduct.SoLanXem;
            product.NgayTao = oldProduct.NgayTao;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ================== DELETE ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!string.IsNullOrEmpty(product.Hinh))
            {
                string path = Path.Combine(
                    _env.WebRootPath,
                    "images/products",
                    product.Hinh
                );

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
