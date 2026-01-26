using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Do_an_lap_trinh_c_.Data;

public partial class Product
{
    public int MaSanPham { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập tên sản phẩm")]
    public string TenSanPham { get; set; } = null!;

    [Required(ErrorMessage = "Vui lòng chọn danh mục")]
    [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn danh mục")]
    public int MaLoai { get; set; }

    public string? MoTaTomTat { get; set; }

    public string? MoTaChiTiet { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn 0")]
    public int DonGia { get; set; }

    public string? Hinh { get; set; }

    public byte SanPhamMoi { get; set; }

    public int SoLanXem { get; set; }

    public DateOnly? NgayTao { get; set; }

    public virtual ICollection<Infobill> Infobills { get; set; } = new List<Infobill>();

    public virtual ICollection<Infobilluser> Infobillusers { get; set; } = new List<Infobilluser>();

    public virtual Typeproduct MaLoaiNavigation { get; set; } = null!;
}
