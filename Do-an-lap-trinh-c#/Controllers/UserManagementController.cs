using Microsoft.AspNetCore.Mvc;
using Do_an_lap_trinh_c_.Data;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly LinhkienpcContext _context;

        public UserManagementController(LinhkienpcContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View("~/Views/Admin/userManagement.cshtml", _context.Users.ToList());
        }

        // ================= ADD =================
        [HttpPost]
        public IActionResult Add(string TenDangNhap, string Email, string MatKhau, byte MaLoaiNguoiDung)
        {
            var user = new User
            {
                TenDangNhap = TenDangNhap,
                HoTen = TenDangNhap, // ✅ FIX LỖI DB NOT NULL
                MatKhau = MatKhau,
                Email = Email,
                MaLoaiNguoiDung = MaLoaiNguoiDung,
                NgayDangKy = DateOnly.FromDateTime(DateTime.Now),
                Active = (byte)1
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        [HttpPost]
        public IActionResult Edit(int MaNguoiDung, string TenDangNhap, string Email, byte MaLoaiNguoiDung)
        {
            var user = _context.Users.Find(MaNguoiDung);
            if (user == null) return NotFound();

            user.TenDangNhap = TenDangNhap;
            user.HoTen = TenDangNhap; // vẫn đảm bảo NOT NULL
            user.Email = Email;
            user.MaLoaiNguoiDung = MaLoaiNguoiDung;

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
