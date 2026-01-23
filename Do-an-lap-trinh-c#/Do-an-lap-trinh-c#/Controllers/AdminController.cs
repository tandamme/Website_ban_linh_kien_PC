using Microsoft.AspNetCore.Mvc;
using Do_an_lap_trinh_c_.Extensions;

public class AdminController : Controller
{
    // Trang mặc định: /Admin
    public IActionResult Index()
    {
        if (!HttpContext.IsLoggedIn())
            return RedirectToAction("Login", "Account");

        if (!HttpContext.IsAdmin())
            return RedirectToAction("Index", "Home");

        return RedirectToAction("ProductManagement");
    }

    public IActionResult ProductManagement()
    {
        CheckAdmin();
        return View();
        // Views/Admin/productManagement.cshtml
    }

    public IActionResult AddProduct()
    {
        CheckAdmin();
        return View();
    }

    public IActionResult EditProduct(int id)
    {
        CheckAdmin();
        return View();
    }

    public IActionResult CategoryManagement()
    {
        CheckAdmin();
        return View();
    }

    public IActionResult UserManagement()
    {
        CheckAdmin();
        return View();
    }

    // ======================
    // Helper check quyền
    private void CheckAdmin()
    {
        if (!HttpContext.IsLoggedIn())
            RedirectToAction("Login", "Account");

        if (!HttpContext.IsAdmin())
            RedirectToAction("Index", "Home");
    }
}
