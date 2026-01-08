using Microsoft.AspNetCore.Mvc;
using Do_an_lap_trinh_c_.Models;
using Do_an_lap_trinh_c_.ViewModels;
using Do_an_lap_trinh_c_.Utils;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        // ====== Constructor ======
        public UserController(AppDbContext context)
        {
            _context = context;
        }

        // ================= Đăng ký =================
        [HttpGet]
        public IActionResult Register()
        {
            return View("~/Views/Admin/signup.cshtml");
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check trùng username
            var exists = _context.Users
                .Any(u => u.user_Name == model.userName);

            if (exists)
            {
                ModelState.AddModelError("", "Tên người dùng đã tồn tại!");
                return View(model);
            }

            string key = MyUtils.keyGenerator();

            var user = new User
            {
                user_Name = model.userName,
                email = model.email,
                randomKey = key,
                passWord = MyUtils.ToMd5Hash(model.passWord, key)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Login");
        }

        // ================= Đăng nhập =================
        [HttpGet]
        public IActionResult Login(string? ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View("~/Views/Admin/login.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users
                .FirstOrDefault(u => u.user_Name == model.userName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(model);
            }

            string hashPassword =
                MyUtils.ToMd5Hash(model.passWord, user.randomKey);

            if (user.passWord != hashPassword)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View(model);
            }

            // ===== Tạo CLAIMS =====
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.user_Name),
                new Claim(ClaimTypes.Email, user.email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var identity = new ClaimsIdentity(
                claims,
                "login"
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return Redirect(ReturnUrl ?? "/");
        }

        // ================= Đăng xuất =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
