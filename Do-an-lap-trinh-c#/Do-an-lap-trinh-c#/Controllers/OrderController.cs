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
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return PlaceOrderCustomer(model); // Gọi hàm đặt hàng cho khách

            var cart = HttpContext.Session.GetObject<List<CartItem>>("Cart.Session");
            if (cart == null || cart.Count == 0)
                return RedirectToAction("Index", "Cart");

            double total = cart.Sum(x => x.Price * x.Quantity);

            // 1️⃣ TẠO ĐƠN HÀNG CHO USER
            var billUser = new Billuser
            {
                NgayHd = DateOnly.FromDateTime(DateTime.Now),
                MaUser = userId.Value,      // ✅ ĐÚNG TÊN
                TriGia = total,
                TrangThai = 0,              // 0 = chờ xử lý
                DiaChi = model.Address
            };

            _context.Billusers.Add(billUser);
            _context.SaveChanges();

            // 2️⃣ CHI TIẾT ĐƠN HÀNG
            foreach (var item in cart)
            {
                _context.Infobillusers.Add(new Infobilluser
                {
                    SoHoaDon = billUser.SoHoaDon,
                    MaSanPham = item.ProductId,
                    SoLuong = item.Quantity,
                    DonGia = item.Price
                });
            }

            _context.SaveChanges();

            // 3️⃣ GỬI MAIL
            var user = _context.Users.Find(userId.Value);
            if (!string.IsNullOrEmpty(user?.Email))
            {
                SendMail(user.Email, billUser.SoHoaDon, cart, total);
            }
            // TempData để hiển thị trang success
            TempData["FullName"] = model.FullName;
            TempData["Email"] = model.Email;
            TempData["PaymentMethod"] = model.PaymentMethod ?? "Thanh toán khi nhận hàng";  

            HttpContext.Session.Remove("Cart.Session");

            return RedirectToAction("CheckSuccess", new { id = billUser.SoHoaDon });
        }


        [HttpPost]
        public IActionResult PlaceOrderCustomer(CheckoutVM model)
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
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
            {
                // 👉 NẾU LÀ KHÁCH (CHƯA LOGIN)
                var bill2 = _context.Bills
                    .Include(b => b.MaKhachHangNavigation)
                    .FirstOrDefault(b => b.SoHoaDon == id);

                if (bill2 == null)
                    return NotFound();

                return View("CheckOutSuccess", bill2);
            }
           

            var bill = _context.Billusers
                .Include(b => b.MaUserNavigation)
                .FirstOrDefault(b => b.SoHoaDon == id && b.MaUser == userId);

            if (bill == null)   
                return NotFound();

            return View(bill);
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



        public IActionResult MyOrders()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Billusers
                .Where(x => x.MaUser == userId)
                .OrderByDescending(x => x.NgayHd)
                .ToList();

            return View(orders);
        }

        // GET: /Order/Details/5

        [Route("Order/OrderDetail/{id}")]
        public IActionResult Details(int id)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var billUser = _context.Billusers
                .Include(b => b.Infobillusers)
                    .ThenInclude(i => i.MaSanPhamNavigation)
                .FirstOrDefault(b => b.SoHoaDon == id && b.MaUser == userId.Value);

            if (billUser == null)
                return NotFound();

            return View(billUser);
        }
    }
