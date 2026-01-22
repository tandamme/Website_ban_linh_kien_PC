using Do_an_lap_trinh_c_.Data;
using Do_an_lap_trinh_c_.Extensions;
using Do_an_lap_trinh_c_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;

public class OrderController : Controller
{
    private readonly LinhkienpcContext _context;

    public OrderController(LinhkienpcContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult PlaceOrder(CheckoutVM model)
{
    if (!ModelState.IsValid)
        return RedirectToAction("CheckOut", "Cart");

    var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart.Session");
    if (cart == null || cart.Count == 0)
        return RedirectToAction("Index", "Cart");

    var customer = new Customer
    {
        TenKhachHang = model.FullName,
        Email = model.Email,
        DienThoai = model.Phone,
        DiaChi = model.Address
    };

    _context.Customers.Add(customer);
    _context.SaveChanges();

    decimal total = cart.Sum(x => x.Price * x.Quantity);

        var bill = new Bill
        {
            MaKhachHang = customer.MaKhachHang,
            NgayHd = DateOnly.FromDateTime(DateTime.Now),
            TriGia = (double)total
        };


        _context.Bills.Add(bill);
    _context.SaveChanges();

    foreach (var item in cart)
    {
        _context.Infobills.Add(new Infobill
        {
            SoHoaDon = bill.SoHoaDon,
            MaSanPham = item.ProductId,
            SoLuong = item.Quantity,
            DonGia = item.Price
        });
    }

    _context.SaveChanges();

        SendMail(model.Email, bill.SoHoaDon, cart, (double)total);

        HttpContext.Session.Remove("Cart.Session");

        // TempData để hiển thị trang success
        TempData["FullName"] = model.FullName;
        TempData["Email"] = model.Email;
        TempData["PaymentMethod"] = model.PaymentMethod ?? "Thanh toán khi nhận hàng";

        return RedirectToAction("CheckSuccess", new { id = bill.SoHoaDon });

    }




    public IActionResult CheckSuccess(int id)
    {
        ViewBag.BillId = id;
        return View();
    }

    // ================= MAIL =================
    private void SendMail(string to, int billId, List<CartItem> cart, double total)
    {
        var body = $@"
        <h2>HÓA ĐƠN #{billId}</h2>
        <p>Cảm ơn bạn đã mua hàng tại PC MASTER</p>
        <hr/>
    ";

        foreach (var item in cart)
        {
            body += $"{item.Name} - {item.Quantity} x {item.Price:#,##0}đ <br/>";
        }

        body += $@"
        <hr/>
        <h3>Tổng tiền: {total:#,##0}đ</h3>
        <p>PC MASTER sẽ liên hệ giao hàng sớm nhất.</p>
    ";

        var smtp = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential(
                "thutruong26022002@gmail.com",
                "asutmxbgeewjkrog"
            ),
            EnableSsl = true
        };

        var mail = new MailMessage
        {
            From = new MailAddress("thutruong26022002@gmail.com", "PC MASTER"),
            Subject = "HÓA ĐƠN MUA HÀNG - PC MASTER",
            Body = body,
            IsBodyHtml = true
        };

        mail.To.Add(to);

        smtp.Send(mail);
    }

}
