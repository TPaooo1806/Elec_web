using Elec_web.Models; // Đảm bảo bạn đã tạo các class Model tương ứng trong folder này
using ElectronicsStore.Models.Entities;
using Microsoft.EntityFrameworkCore;
namespace Elec_web
{
    public class ElecDbContexts : DbContext
    {
        public ElecDbContexts(DbContextOptions<ElecDbContexts> options) : base(options)
        {
        }

        // --- KHAI BÁO CÁC BẢNG (DbSets) ---
        public DbSet<DanhMucSanPham> DanhMucSanPhams { get; set; }
        public DbSet<NhaSanXuat> NhaSanXuats { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
        public DbSet<GioHang> GioHangs { get; set; }
        public DbSet<DonHang> DonHangs { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public DbSet<DanhGiaSanPham> DanhGiaSanPhams { get; set; }
        public DbSet<MaKhuyenMai> MaKhuyenMais { get; set; }
        public DbSet<LichSuThanhToan> LichSuThanhToans { get; set; }
        public DbSet<LichSuDonHang> LichSuDonHangs { get; set; }
        public DbSet<ThongKeLuotXem> ThongKeLuotXems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Danh mục & Sản phẩm
            modelBuilder.Entity<DanhMucSanPham>()
                .HasMany(d => d.SanPhams)
                .WithOne(s => s.DanhMuc)
                .HasForeignKey(s => s.MaDanhMuc)
                .OnDelete(DeleteBehavior.Restrict);

            // 2. Nhà sản xuất & Sản phẩm
            modelBuilder.Entity<NhaSanXuat>()
                .HasMany(n => n.SanPhams)
                .WithOne(s => s.NhaSX)
                .HasForeignKey(s => s.MaNhaSX)
                .OnDelete(DeleteBehavior.SetNull);

            // 3. Người dùng & Đơn hàng/Giỏ hàng
            modelBuilder.Entity<NguoiDung>()
                .HasMany(n => n.DonHangs)
                .WithOne(d => d.NguoiDung)
                .HasForeignKey(d => d.MaND)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NguoiDung>()
                .HasMany(n => n.GioHangs)
                .WithOne(g => g.NguoiDung)
                .HasForeignKey(g => g.MaND)
                .OnDelete(DeleteBehavior.Cascade);

            // 4. Giỏ hàng (Unique constraint cho MaND và MaSP)
            modelBuilder.Entity<GioHang>()
                .HasIndex(g => new { g.MaND, g.MaSP })
                .IsUnique();

            // 5. Đơn hàng & Chi tiết đơn hàng
            modelBuilder.Entity<DonHang>()
                .HasMany(d => d.ChiTietDonHangs)
                .WithOne(c => c.DonHang)
                .HasForeignKey(c => c.MaDH)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DonHang>()
                .HasIndex(d => d.MaDonHangGoc)
                .IsUnique();

            // 6. Sản phẩm & Đánh giá
            modelBuilder.Entity<SanPham>()
                .HasMany(s => s.DanhGias)
                .WithOne(d => d.SanPham)
                .HasForeignKey(d => d.MaSP)
                .OnDelete(DeleteBehavior.Cascade);

            // 7. Các ràng buộc Duy nhất (Unique)
            modelBuilder.Entity<NguoiDung>()
                .HasIndex(n => n.Email)
                .IsUnique();

            modelBuilder.Entity<MaKhuyenMai>()
                .HasIndex(m => m.MaCode)
                .IsUnique();
        }
    }
}