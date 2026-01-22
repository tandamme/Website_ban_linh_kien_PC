using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Billuser
{
    public int SoHoaDon { get; set; }

    public DateOnly? NgayHd { get; set; }

    public int? MaUser { get; set; }

    public double? TriGia { get; set; }

    public byte? TrangThai { get; set; }

    public string? DiaChi { get; set; }

    public virtual ICollection<Infobilluser> Infobillusers { get; set; } = new List<Infobilluser>();

    public virtual User? MaUserNavigation { get; set; }
}
