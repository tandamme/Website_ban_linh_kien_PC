using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Do_an_lap_trinh_c_.Data;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class TestController : Controller
    {
        private readonly LinhkienpcContext _context;

        public TestController(LinhkienpcContext context)
        {
            _context = context;
        }

        // GET /test/db
        [HttpGet("/test/db")]
        public async Task<IActionResult> DbTest()
        {
            try
            {
                // Simple checks: count products and return one example
                var productCount = await _context.Products.CountAsync();
                var sample = await _context.Products
                    .AsNoTracking()
                    .Include(p => p.MaLoaiNavigation)
                    .FirstOrDefaultAsync();

                return Json(new
                {
                    ok = true,
                    productCount,
                    sample = sample == null ? null : new
                    {
                        id = sample.MaSanPham,
                        name = sample.TenSanPham,
                        price = sample.DonGia,
                        category = sample.MaLoaiNavigation?.TenLoai
                    }
                });
            }
            catch (Exception ex)
            {
                // Return error details to help debug connection issues
                return StatusCode(500, new
                {
                    ok = false,
                    message = "Failed to query database",
                    error = ex.Message,
                    exceptionType = ex.GetType().FullName
                });
            }
        }
    }
}