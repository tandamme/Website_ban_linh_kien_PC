using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Infobill
{
    public int Stt { get; set; }

    public int SoHoaDon { get; set; }

    public int MaSanPham { get; set; }

    public int SoLuong { get; set; }

    public int DonGia { get; set; }

    public virtual Product MaSanPhamNavigation { get; set; } = null!;

    public virtual Bill SoHoaDonNavigation { get; set; } = null!;
}
