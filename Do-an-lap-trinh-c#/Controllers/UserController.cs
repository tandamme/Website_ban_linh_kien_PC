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
            // 1. Check Required
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 2. Check username tồn tại hay chưa
            var user = _context.Users
                .FirstOrDefault(u => u.user_Name == model.userName);

            if (user == null)
            {
                ModelState.AddModelError(
                    "userName",
                    "Tên người dùng chưa được đăng ký"
                );
                return View(model);
            }

            // 3. Check password
            string hashPassword =
                MyUtils.ToMd5Hash(model.passWord, user.randomKey);

            if (user.passWord != hashPassword)
            {
                ModelState.AddModelError(
                    "passWord",
                    "Mật khẩu không chính xác"
                );
                return View(model);
            }

            // 4. Đăng nhập thành công
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.user_Name),
        new Claim(ClaimTypes.Email, user.email),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

            var identity = new ClaimsIdentity(claims, "login");
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

        // ===== AJAX VALIDATION =====
        [HttpGet]
        public IActionResult CheckUsername(string userName)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.user_Name == userName);

            if (user != null)
                return Content("Tên đăng nhập đã tồn tại");

            return Content("OK");
        }


        [HttpGet]
        public IActionResult CheckEmail(string email)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.email == email);

            if (user != null)
                return Content("Email đã tồn tại");

            return Content("OK");
        }


        // ===== CHECK USERNAME LOGIN =====
        [HttpGet]
        public IActionResult CheckLoginUsername(string userName)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.user_Name == userName);

            if (user == null)
                return Content("Tên người dùng chưa được đăng ký");

            return Content("OK");
        }

        // ===== CHECK PASSWORD LOGIN =====
        [HttpGet]
        public IActionResult CheckLoginPassword(string userName, string passWord)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.user_Name == userName);

            if (user == null)
                return Content("Tên người dùng chưa được đăng ký");

            string hash = MyUtils.ToMd5Hash(passWord, user.randomKey);

            if (user.passWord != hash)
                return Content("Mật khẩu không chính xác");

            return Content("OK");
        }


    }
}
