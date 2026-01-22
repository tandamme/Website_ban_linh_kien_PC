using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class User
{
    public int MaNguoiDung { get; set; }

    public byte MaLoaiNguoiDung { get; set; }

    public string HoTen { get; set; } = null!;

    public string TenDangNhap { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public DateOnly? NgayDangKy { get; set; }

    public DateOnly? NgayDangNhapCuoi { get; set; }

    public byte Active { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<Billuser> Billusers { get; set; } = new List<Billuser>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
