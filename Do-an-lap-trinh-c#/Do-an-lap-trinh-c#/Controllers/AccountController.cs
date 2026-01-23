namespace Do_an_lap_trinh_c_.Controllers;

using Do_an_lap_trinh_c_.Data;
using Do_an_lap_trinh_c_.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Cryptography;
using System.Text;

public class AccountController : Controller
{
    private readonly LinhkienpcContext _context;

    public AccountController(LinhkienpcContext context)
    {
        _context = context;
    }

    // GET: /Account/Register
    public IActionResult Register()
    {
        return View("~/Views/Account/signup.cshtml");
    }

    // POST: /Account/Register
    [HttpPost]
    public IActionResult Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View("~/Views/Account/signup.cshtml", model);

        // kiểm tra trùng TÊN ĐĂNG NHẬP
        if (_context.Users.Any(u => u.TenDangNhap == model.TenDangNhap))
        {
            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập đã tồn tại");
            return View("~/Views/Account/signup.cshtml", model);
        }

        // kiểm tra trùng EMAIL (nếu có nhập)
        if (!string.IsNullOrEmpty(model.Email) &&
            _context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email đã được sử dụng");
            return View("~/Views/Account/signup.cshtml", model);
        }

        // ✔ tạo user mới
        var user = new User
        {
            HoTen = model.HoTen,
            TenDangNhap = model.TenDangNhap,
            MatKhau = HashPassword(model.MatKhau),
            Email = model.Email,
            MaLoaiNguoiDung = 1,
            Active = 1,
            NgayDangKy = DateOnly.FromDateTime(DateTime.Now)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    // HASH PASSWORD
    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    [HttpGet]
    public IActionResult Login(string? ReturnUrl)
    {
        ViewBag.ReturnUrl = ReturnUrl;
        return View("~/Views/Account/login.cshtml");
    }

    [HttpPost]
    public IActionResult Login(LoginVM model, string? ReturnUrl)
    {
        if (!ModelState.IsValid)
            return View(model);

        // kiểm tra username tồn tại chưa
        var user = _context.Users
            .FirstOrDefault(u => u.TenDangNhap == model.TenDangNhap);

        if (user == null)
        {
            ModelState.AddModelError("TenDangNhap", "Tên đăng nhập không tồn tại");
            return View(model);
        }

        // kiểm tra tài khoản có bị khóa không
        if (user.Active == 0)
        {
            ModelState.AddModelError("", "Tài khoản đã bị khóa");
            return View(model);
        }

        // kiểm tra mật khẩu
        string passwordHash = HashPassword(model.MatKhau);

        if (user.MatKhau != passwordHash)
        {
            ModelState.AddModelError("MatKhau", "Mật khẩu không đúng");
            return View(model);
        }

        // đăng nhập thành công → lưu session
        HttpContext.Session.SetInt32("UserId", user.MaNguoiDung);
        HttpContext.Session.SetString("UserName", user.TenDangNhap);
        HttpContext.Session.SetString("FullName", user.HoTen);
        HttpContext.Session.SetInt32("Role", user.MaLoaiNguoiDung);

        if (!string.IsNullOrEmpty(ReturnUrl))
            return Redirect(ReturnUrl);

        return RedirectToAction("Home", "Home");
    }



    [HttpGet]
    public IActionResult CheckUsername(string tenDangNhap)
    {
        bool exists = _context.Users.Any(u => u.TenDangNhap == tenDangNhap);
        return Content(exists ? "Tên đăng nhập đã tồn tại" : "OK");
    }

    [HttpGet]
    public IActionResult CheckEmail(string email)
    {
        bool exists = _context.Users.Any(u => u.Email == email);
        return Content(exists ? "Email đã được sử dụng" : "OK");
    }


    // CHECK USERNAME LOGIN
    [HttpGet]
    public IActionResult CheckLoginUsername(string tenDangNhap)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.TenDangNhap == tenDangNhap);

        if (user == null)
            return Content("Tên đăng nhập không tồn tại");

        if (user.Active == 0)
            return Content("Tài khoản đã bị khóa");

        return Content("OK");
    }

    // CHECK PASSWORD LOGIN
    [HttpGet]
    public IActionResult CheckLoginPassword(string tenDangNhap, string matKhau)
    {
        var user = _context.Users
            .FirstOrDefault(u => u.TenDangNhap == tenDangNhap);

        if (user == null)
            return Content("OK"); // để username xử lý

        var hash = HashPassword(matKhau);

        if (user.MatKhau != hash)
            return Content("Mật khẩu không đúng");

        return Content("OK");
    }

}