using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_an_lap_trinh_c_.Data;

namespace Do_an_lap_trinh_c_.Controllers
{
    public class ProductsController : Controller
    {
        private readonly LinhkienpcContext _context;

        public ProductsController(LinhkienpcContext context)
        {
            _context = context;
        }

        // GET: Products
        public IActionResult Index(int? loai)
        {
            var data = _context.Products.AsQueryable();
            if (loai != null)
            {
                data = data.Where(p => p.MaLoai == loai);
            }
            var list = data
                .Include(p => new Product{
                    MaLoai = p.MaLoai,
                    MaSanPham = p.MaSanPham,
                    TenSanPham = p.TenSanPham,
                    DonGia = p.DonGia,
                    Hinh = p.Hinh

                });
                
            return View(list);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.Typeproducts, "MaLoai", "MaLoai");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaSanPham,TenSanPham,MaLoai,MoTaTomTat,MoTaChiTiet,DonGia,Hinh,SanPhamMoi,SoLanXem,NgayTao")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.Typeproducts, "MaLoai", "MaLoai", product.MaLoai);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["MaLoai"] = new SelectList(_context.Typeproducts, "MaLoai", "MaLoai", product.MaLoai);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaSanPham,TenSanPham,MaLoai,MoTaTomTat,MoTaChiTiet,DonGia,Hinh,SanPhamMoi,SoLanXem,NgayTao")] Product product)
        {
            if (id != product.MaSanPham)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.MaSanPham))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaLoai"] = new SelectList(_context.Typeproducts, "MaLoai", "MaLoai", product.MaLoai);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.MaLoaiNavigation)
                .FirstOrDefaultAsync(m => m.MaSanPham == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.MaSanPham == id);
        }
    }
}
