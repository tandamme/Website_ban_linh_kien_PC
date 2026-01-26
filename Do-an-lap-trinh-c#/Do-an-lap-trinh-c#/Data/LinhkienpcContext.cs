using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Do_an_lap_trinh_c_.Data;

public partial class LinhkienpcContext : DbContext
{
    public LinhkienpcContext()
    {
    }

    public LinhkienpcContext(DbContextOptions<LinhkienpcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Bill> Bills { get; set; }

    public virtual DbSet<Billuser> Billusers { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Infobill> Infobills { get; set; }

    public virtual DbSet<Infobilluser> Infobillusers { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Typecustomer> Typecustomers { get; set; }

    public virtual DbSet<Typepost> Typeposts { get; set; }

    public virtual DbSet<Typeproduct> Typeproducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost\\SQLEXPRESS;Initial Catalog=linhkienpc;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bill>(entity =>
        {
            entity.HasKey(e => e.SoHoaDon).HasName("PK__bill__7E96EB13834E9316");

            entity.ToTable("bill");

            entity.HasIndex(e => e.MaKhachHang, "IX_bill_ma_khach_hang");

            entity.Property(e => e.SoHoaDon).HasColumnName("so_hoa_don");
            entity.Property(e => e.MaKhachHang).HasColumnName("ma_khach_hang");
            entity.Property(e => e.NgayHd).HasColumnName("ngay_hd");
            entity.Property(e => e.TriGia).HasColumnName("tri_gia");

            entity.HasOne(d => d.MaKhachHangNavigation).WithMany(p => p.Bills)
                .HasForeignKey(d => d.MaKhachHang)
                .HasConstraintName("FK_bill_customer");
        });

        modelBuilder.Entity<Billuser>(entity =>
        {
            entity.HasKey(e => e.SoHoaDon).HasName("PK__billuser__7E96EB135E784F68");

            entity.ToTable("billuser");

            entity.HasIndex(e => e.MaUser, "IX_billuser_ma_user");

            entity.Property(e => e.SoHoaDon).HasColumnName("so_hoa_don");
            entity.Property(e => e.DiaChi).HasColumnName("dia_chi");
            entity.Property(e => e.MaUser).HasColumnName("ma_user");
            entity.Property(e => e.NgayHd).HasColumnName("ngay_hd");
            entity.Property(e => e.TrangThai).HasColumnName("trang_thai");
            entity.Property(e => e.TriGia).HasColumnName("tri_gia");

            entity.HasOne(d => d.MaUserNavigation).WithMany(p => p.Billusers)
                .HasForeignKey(d => d.MaUser)
                .HasConstraintName("FK_billuser_user");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.MaKhachHang).HasName("PK__customer__C9817AF6AE6E47B5");

            entity.ToTable("customer");

            entity.Property(e => e.MaKhachHang).HasColumnName("ma_khach_hang");
            entity.Property(e => e.DiaChi)
                .HasMaxLength(200)
                .HasColumnName("dia_chi");
            entity.Property(e => e.DienThoai)
                .HasMaxLength(20)
                .HasColumnName("dien_thoai");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
       
            entity.Property(e => e.TenKhachHang)
                .HasMaxLength(100)
                .HasColumnName("ten_khach_hang");
        });

        modelBuilder.Entity<Infobill>(entity =>
        {
            entity.HasKey(e => e.Stt).HasName("PK__infobill__DDDF328EA859CC57");

            entity.ToTable("infobill");

            entity.HasIndex(e => e.MaSanPham, "IX_infobill_ma_san_pham");

            entity.HasIndex(e => e.SoHoaDon, "IX_infobill_so_hoa_don");

            entity.Property(e => e.Stt).HasColumnName("stt");
            entity.Property(e => e.DonGia).HasColumnName("don_gia");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.SoHoaDon).HasColumnName("so_hoa_don");
            entity.Property(e => e.SoLuong).HasColumnName("so_luong");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.Infobills)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_infobill_product");

            entity.HasOne(d => d.SoHoaDonNavigation).WithMany(p => p.Infobills)
                .HasForeignKey(d => d.SoHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_infobill_bill");
        });

        modelBuilder.Entity<Infobilluser>(entity =>
        {
            entity.HasKey(e => e.Stt).HasName("PK__infobill__DDDF328ED861C3CE");

            entity.ToTable("infobilluser");

            entity.HasIndex(e => e.MaSanPham, "IX_infobilluser_ma_san_pham");

            entity.HasIndex(e => e.SoHoaDon, "IX_infobilluser_so_hoa_don");

            entity.Property(e => e.Stt).HasColumnName("stt");
            entity.Property(e => e.DonGia).HasColumnName("don_gia");
            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.SoHoaDon).HasColumnName("so_hoa_don");
            entity.Property(e => e.SoLuong).HasColumnName("so_luong");

            entity.HasOne(d => d.MaSanPhamNavigation).WithMany(p => p.Infobillusers)
                .HasForeignKey(d => d.MaSanPham)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_infobilluser_product");

            entity.HasOne(d => d.SoHoaDonNavigation).WithMany(p => p.Infobillusers)
                .HasForeignKey(d => d.SoHoaDon)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_infobilluser_billuser");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.MaBaiViet).HasName("PK__post__E949555760B5F0E3");

            entity.ToTable("post");

            entity.HasIndex(e => e.MaLoaiBaiViet, "IX_post_ma_loai_bai_viet");

            entity.HasIndex(e => e.MaNguoiDung, "IX_post_ma_nguoi_dung");

            entity.Property(e => e.MaBaiViet).HasColumnName("ma_bai_viet");
            entity.Property(e => e.MaLoaiBaiViet).HasColumnName("ma_loai_bai_viet");
            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.NgayGuiBai)
                .HasColumnType("datetime")
                .HasColumnName("ngay_gui_bai");
            entity.Property(e => e.NgayHetHan)
                .HasColumnType("datetime")
                .HasColumnName("ngay_het_han");
            entity.Property(e => e.NgayXuatBan)
                .HasColumnType("datetime")
                .HasColumnName("ngay_xuat_ban");
            entity.Property(e => e.NoiDungChiTiet).HasColumnName("noi_dung_chi_tiet");
            entity.Property(e => e.NoiDungTomTat).HasColumnName("noi_dung_tom_tat");
            entity.Property(e => e.SoLanXem).HasColumnName("so_lan_xem");
            entity.Property(e => e.TieuDe)
                .HasMaxLength(200)
                .HasColumnName("tieu_de");
            entity.Property(e => e.XuatBan).HasColumnName("xuat_ban");

            entity.HasOne(d => d.MaLoaiBaiVietNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.MaLoaiBaiViet)
                .HasConstraintName("FK_post_typepost");

            entity.HasOne(d => d.MaNguoiDungNavigation).WithMany(p => p.Posts)
                .HasForeignKey(d => d.MaNguoiDung)
                .HasConstraintName("FK_post_user");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.MaSanPham).HasName("PK__product__9D25990C843D7905");

            entity.ToTable("product");

            entity.HasIndex(e => e.MaLoai, "IX_product_ma_loai");

            entity.Property(e => e.MaSanPham).HasColumnName("ma_san_pham");
            entity.Property(e => e.DonGia).HasColumnName("don_gia");
            entity.Property(e => e.Hinh)
                .HasMaxLength(200)
                .HasColumnName("hinh");
            entity.Property(e => e.MaLoai).HasColumnName("ma_loai");
            entity.Property(e => e.MoTaChiTiet).HasColumnName("mo_ta_chi_tiet");
            entity.Property(e => e.MoTaTomTat).HasColumnName("mo_ta_tom_tat");
            entity.Property(e => e.NgayTao).HasColumnName("ngay_tao");
            entity.Property(e => e.SanPhamMoi).HasColumnName("san_pham_moi");
            entity.Property(e => e.SoLanXem).HasColumnName("so_lan_xem");
            entity.Property(e => e.TenSanPham)
                .HasMaxLength(100)
                .HasColumnName("ten_san_pham");

            entity.HasOne(d => d.MaLoaiNavigation).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaLoai)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_product_typeproduct");
        });

        modelBuilder.Entity<Typecustomer>(entity =>
        {
            entity.HasKey(e => e.MaLoaiNguoiDung).HasName("PK__typecust__7AD2F3308CD789ED");

            entity.ToTable("typecustomer");

            entity.Property(e => e.MaLoaiNguoiDung)
                .ValueGeneratedOnAdd()
                .HasColumnName("ma_loai_nguoi_dung");
            entity.Property(e => e.TenLoaiNguoiDung)
                .HasMaxLength(100)
                .HasColumnName("ten_loai_nguoi_dung");
        });

        modelBuilder.Entity<Typepost>(entity =>
        {
            entity.HasKey(e => e.MaLoaiBaiViet).HasName("PK__typepost__18707BF561C14C78");

            entity.ToTable("typepost");

            entity.Property(e => e.MaLoaiBaiViet).HasColumnName("ma_loai_bai_viet");
            entity.Property(e => e.MaLoaiCha).HasColumnName("ma_loai_cha");
            entity.Property(e => e.MoTa).HasColumnName("mo_ta");
            entity.Property(e => e.TenLoaiBaiViet)
                .HasMaxLength(50)
                .HasColumnName("ten_loai_bai_viet");
        });

        modelBuilder.Entity<Typeproduct>(entity =>
        {
            entity.HasKey(e => e.MaLoai).HasName("PK__typeprod__D9476E57F4412E79");

            entity.ToTable("typeproduct");

            entity.Property(e => e.MaLoai).HasColumnName("ma_loai");
            entity.Property(e => e.Hinh)
                .HasMaxLength(200)
                .HasColumnName("hinh");
            entity.Property(e => e.MaLoaiCha).HasColumnName("ma_loai_cha");
            entity.Property(e => e.MoTa).HasColumnName("mo_ta");
            entity.Property(e => e.TenLoai)
                .HasMaxLength(50)
                .HasColumnName("ten_loai");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.MaNguoiDung).HasName("PK__User__19C32CF77C911B09");

            entity.ToTable("User");

            entity.Property(e => e.MaNguoiDung).HasColumnName("ma_nguoi_dung");
            entity.Property(e => e.Active).HasColumnName("active");
            entity.Property(e => e.DiaChi).HasColumnName("dia_chi");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .HasColumnName("email");
            entity.Property(e => e.HoTen)
                .HasMaxLength(100)
                .HasColumnName("ho_ten");
            entity.Property(e => e.MaLoaiNguoiDung).HasColumnName("ma_loai_nguoi_dung");
            entity.Property(e => e.MatKhau)
                .HasMaxLength(200)
                .HasColumnName("mat_khau");
            entity.Property(e => e.NgayDangKy).HasColumnName("ngay_dang_ky");
            entity.Property(e => e.NgayDangNhapCuoi).HasColumnName("ngay_dang_nhap_cuoi");
            entity.Property(e => e.TenDangNhap)
                .HasMaxLength(100)
                .HasColumnName("ten_dang_nhap");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
