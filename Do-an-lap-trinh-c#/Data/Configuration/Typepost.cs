using System;
using System.Collections.Generic;

namespace Do_an_lap_trinh_c_.Data;

public partial class Typepost
{
    public int MaLoaiBaiViet { get; set; }

    public string TenLoaiBaiViet { get; set; } = null!;

    public string? MoTa { get; set; }

    public int? MaLoaiCha { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
