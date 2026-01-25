using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Typeproduct
{
    public int MaLoai { get; set; }

    public string TenLoai { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaLoaiCha { get; set; }

    public string? Hinh { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
