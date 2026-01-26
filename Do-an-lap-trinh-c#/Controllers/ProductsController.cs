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

        // Tìm đến đoạn [HttpPost] AddProduct cũ và thay thế bằng đoạn này:

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, IFormFile? ImageFile)
        {
            // 1. Bỏ qua validation cho các trường không nhập từ form
            ModelState.Remove(nameof(Product.SoLanXem));
            ModelState.Remove(nameof(Product.NgayTao));
            ModelState.Remove(nameof(Product.Hinh));

            // QUAN TRỌNG: Bỏ qua MaLoaiNavigation vì khi submit form nó là null, gây lỗi isValid = false
            ModelState.Remove("MaLoaiNavigation");

            // 2. Gán giá trị mặc định
            product.SoLanXem = 0;
            product.NgayTao = DateOnly.FromDateTime(DateTime.Now);

            // 3. Xử lý upload ảnh
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string folderPath = Path.Combine(_env.WebRootPath, "images/products");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                // Tạo tên file ngẫu nhiên để tránh trùng
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                product.Hinh = fileName;
            }
            else
            {
                product.Hinh = "no-image.png"; // Ảnh mặc định nếu không upload
            }

            // 4. Kiểm tra Validation
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
                    // Lưu thành công thì quay về trang quản lý
                    return RedirectToAction("ProductManagement", "Home");
                }
                catch (Exception ex)
                {
                    // Nếu có lỗi SQL hoặc hệ thống, báo lỗi ra view
                    ModelState.AddModelError("", "Lỗi khi lưu dữ liệu: " + ex.Message);
                }
            }

            // 5. Nếu thất bại (Validation sai hoặc Lỗi try-catch), load lại View và Dropdown
            ViewBag.MaLoai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai",
                product.MaLoai
            );

            return View("~/Views/Products/addProduct.cshtml", product);

        }

        // ================== EDIT ==================
        // ==========================================
        // 1. HÀM GET: Dùng để hiển thị form sửa (Chạy khi bấm nút Edit)
        // ==========================================
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            // Tạo dropdown danh mục
            ViewBag.MaLoai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai",
                product.MaLoai
            );

            // Trả về View Edit.cshtml để người dùng nhập liệu
            return View(product);
        }

        // ==========================================
        // 2. HÀM POST: Dùng để lưu dữ liệu (Chạy khi bấm nút Lưu)
        // ==========================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, IFormFile? ImageFile)
        {
            if (id != product.MaSanPham) return NotFound();

            // Bỏ qua validation các trường không nhập từ form
            ModelState.Remove("MaLoaiNavigation");
            ModelState.Remove("Hinh");
            ModelState.Remove("NgayTao");
            ModelState.Remove("SoLanXem");

            if (ModelState.IsValid)
            {
                try
                {
                    var oldProduct = await _context.Products
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.MaSanPham == id);

                    if (oldProduct == null) return NotFound();

                    // --- XỬ LÝ ẢNH ---
                    if (ImageFile != null && ImageFile.Length > 0)
                    {
                        string folder = Path.Combine(_env.WebRootPath, "images/products");

                        // Xóa ảnh cũ
                        if (!string.IsNullOrEmpty(oldProduct.Hinh))
                        {
                            string oldPath = Path.Combine(folder, oldProduct.Hinh);
                            if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                        }

                        // Lưu ảnh mới
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                        string newPath = Path.Combine(folder, fileName);
                        using (var stream = new FileStream(newPath, FileMode.Create))
                        {
                            await ImageFile.CopyToAsync(stream);
                        }
                        product.Hinh = fileName;
                    }
                    else
                    {
                        product.Hinh = oldProduct.Hinh; // Giữ ảnh cũ
                    }

                    // --- GIỮ DATA CŨ ---
                    product.SoLanXem = oldProduct.SoLanXem;
                    product.NgayTao = oldProduct.NgayTao;

                    // --- LƯU DATABASE ---
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();

                    // Chuyển hướng về trang Quản lý
                    return RedirectToAction("ProductManagement", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi cập nhật: " + ex.Message);
                }
            }

            // Nếu lỗi thì load lại dropdown và hiển thị lại form
            ViewBag.MaLoai = new SelectList(
                _context.Typeproducts,
                "MaLoai",
                "TenLoai",
                product.MaLoai
            );
            return View(product);
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

            return RedirectToAction("ProductManagement", "Home");
        }
    }
}
