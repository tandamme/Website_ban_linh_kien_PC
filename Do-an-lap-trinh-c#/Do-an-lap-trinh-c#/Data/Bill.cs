using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Bill
{
    public int SoHoaDon { get; set; }

    public DateOnly? NgayHd { get; set; }

    public int? MaKhachHang { get; set; }

    public double? TriGia { get; set; }

    public virtual ICollection<Infobill> Infobills { get; set; } = new List<Infobill>();

    public virtual Customer? MaKhachHangNavigation { get; set; }
}
