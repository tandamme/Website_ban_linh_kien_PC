using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Post
{
    public int MaBaiViet { get; set; }

    public int? MaLoaiBaiViet { get; set; }

    public int? MaNguoiDung { get; set; }

    public string? TieuDe { get; set; }

    public string? NoiDungTomTat { get; set; }

    public string? NoiDungChiTiet { get; set; }

    public DateTime? NgayGuiBai { get; set; }

    public DateTime? NgayXuatBan { get; set; }

    public DateTime? NgayHetHan { get; set; }

    public int SoLanXem { get; set; }

    public byte XuatBan { get; set; }

    public virtual Typepost? MaLoaiBaiVietNavigation { get; set; }

    public virtual User? MaNguoiDungNavigation { get; set; }
}
