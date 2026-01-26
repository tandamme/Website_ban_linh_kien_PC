using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Customer
{
    public int MaKhachHang { get; set; }

    public string TenKhachHang { get; set; } = null!;



    public string? DiaChi { get; set; }

    public string? DienThoai { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();
}
