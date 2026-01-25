using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Product
{
    public int MaSanPham { get; set; }

    public string TenSanPham { get; set; } = null!;

    public int MaLoai { get; set; }

    public string? MoTaTomTat { get; set; }

    public string? MoTaChiTiet { get; set; }

    public int DonGia { get; set; }

    public string? Hinh { get; set; }

    public int SanPhamMoi { get; set; }

    public int SoLanXem { get; set; }

    public DateOnly? NgayTao { get; set; }

    public virtual ICollection<Infobill> Infobills { get; set; } = new List<Infobill>();

    public virtual ICollection<Infobilluser> Infobillusers { get; set; } = new List<Infobilluser>();

    public virtual Typeproduct MaLoaiNavigation { get; set; } = null!;
}
