using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Common;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModels
{
    public partial class Web_BookingTrans_TestContext : DbContext
    {
        public Web_BookingTrans_TestContext()
        {
        }

        public Web_BookingTrans_TestContext(DbContextOptions<Web_BookingTrans_TestContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CompanyModel> CompanyModel { get; set; }
        public virtual DbSet<CungDuongModel> CungDuongModel { get; set; }
        public virtual DbSet<CustomerModel> CustomerModel { get; set; }
        public virtual DbSet<DriverRegister> DriverRegister { get; set; }
        public virtual DbSet<OrderMapping> OrderMapping { get; set; }
        public virtual DbSet<PogiaKhacMapping> PogiaKhacMapping { get; set; }
        public virtual DbSet<PolineModel> PolineModel { get; set; }
        public virtual DbSet<PomasterModel> PomasterModel { get; set; }
        public virtual DbSet<ProviderModel> ProviderModel { get; set; }
        public virtual DbSet<SolineModel> SolineModel { get; set; }
        public virtual DbSet<SomasterModel> SomasterModel { get; set; }
        public virtual DbSet<SyncTable> SyncTable { get; set; }
        public virtual DbSet<TransportModel> TransportModel { get; set; }
        public virtual DbSet<UserModel> UserModel { get; set; }
        public virtual DbSet<VehicleModel> VehicleModel { get; set; }
        public virtual DbSet<VehicleOwnerModel> VehicleOwnerModel { get; set; }
        public virtual DbSet<VehicleRegisterMobileModel> VehicleRegisterMobileModel { get; set; }
        public virtual DbSet<VehicleRegisterPodetailModel> VehicleRegisterPodetailModel { get; set; }
        public virtual DbSet<VehicleVehicleOwnerMapping> VehicleVehicleOwnerMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.getInstance().connWeb);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CompanyCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CompanyName).HasMaxLength(200);
            });

            modelBuilder.Entity<CungDuongModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CungDuongName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.KhoangCach).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Plant)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Address).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.CungDuongName).HasMaxLength(500);

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<DriverRegister>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DriverCardNo)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DriverName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedTime).HasColumnType("datetime");

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<OrderMapping>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.BillNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.MappingId).HasColumnName("MappingID");

                entity.Property(e => e.MasterId).HasColumnName("MasterID");

                entity.Property(e => e.OrderNumber).HasMaxLength(50);

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.SoLuong).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.SoLuongCont).HasColumnType("decimal(10, 0)");

                entity.Property(e => e.StartDate).HasColumnType("date");
            });

            modelBuilder.Entity<PogiaKhacMapping>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("POGiaKhacMapping");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.PoNumber)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PolineModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("POLineModel");

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.DocumentDate).HasColumnType("date");

                entity.Property(e => e.IsDeliveryCompleted).HasColumnName("isDeliveryCompleted");

                entity.Property(e => e.IsPmccompleted).HasColumnName("isPMCCompleted");

                entity.Property(e => e.IsRelease).HasColumnName("isRelease");

                entity.Property(e => e.IsUnlimited).HasColumnName("isUnlimited");

                entity.Property(e => e.OverTolerance).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Poline)
                    .IsRequired()
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Ponumber)
                    .IsRequired()
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaNhap).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.UnderTolerance).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);

                entity.Property(e => e.WarehouseCode).HasMaxLength(100);
            });

            modelBuilder.Entity<PomasterModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("POMasterModel");

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.IsCompelete).HasColumnName("isCompelete");

                entity.Property(e => e.IsNhapKhau).HasColumnName("isNhapKhau");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.Ponumber)
                    .IsRequired()
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);

                entity.Property(e => e.QtyTotal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaNhap).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<ProviderModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccountGroup).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);
            });

            modelBuilder.Entity<SolineModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SOLineModel");

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.DocumentDate).HasColumnType("date");

                entity.Property(e => e.IsClosed).HasColumnName("isClosed");

                entity.Property(e => e.IsComplete).HasColumnName("isComplete");

                entity.Property(e => e.IsUnlimited).HasColumnName("isUnlimited");

                entity.Property(e => e.OverTolerance).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(35);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaXuat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Soline)
                    .IsRequired()
                    .HasColumnName("SOLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .IsRequired()
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.UnderTolerance).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<SomasterModel>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("SOMasterModel");

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.IsCompelete).HasColumnName("isCompelete");

                entity.Property(e => e.IsNhapKhau).HasColumnName("isNhapKhau");

                entity.Property(e => e.QtyTotal).HasColumnType("decimal(13, 3)");

                entity.Property(e => e.SoLuongDaXuat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Sonumber)
                    .IsRequired()
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SyncTable>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Time)
                    .HasColumnName("time")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<TransportModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.AccountGroup).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.TransportCode).HasMaxLength(100);

                entity.Property(e => e.TransportName).HasMaxLength(1000);
            });

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.Active).HasColumnName("active");

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.Fullname)
                    .HasColumnName("fullname")
                    .HasMaxLength(100);

                entity.Property(e => e.Memberof).HasColumnName("memberof");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(50);

                entity.Property(e => e.Rolecode).HasColumnName("rolecode");

                entity.Property(e => e.Taxcode)
                    .IsRequired()
                    .HasColumnName("taxcode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .HasColumnName("token")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserType).HasMaxLength(50);

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VehicleModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DriverCardNo).HasMaxLength(100);

                entity.Property(e => e.DriverName).HasMaxLength(100);

                entity.Property(e => e.IsContainer).HasColumnName("isContainer");

                entity.Property(e => e.IsDauKeo).HasColumnName("isDauKeo");

                entity.Property(e => e.IsLock).HasColumnName("isLock");

                entity.Property(e => e.IsLockEdit).HasColumnName("isLockEdit");

                entity.Property(e => e.IsRoMooc).HasColumnName("isRoMooc");

                entity.Property(e => e.LastEditTime).HasColumnType("datetime");

                entity.Property(e => e.TempWeight).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TrongLuongDangKiem).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TyLeVuot).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.UpdateTempWeightTime).HasColumnType("datetime");

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);

                entity.Property(e => e.VehicleOwner).HasMaxLength(100);

                entity.Property(e => e.VehicleWeight).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<VehicleOwnerModel>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.VehicleOwner)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.VehicleOwnerName).HasMaxLength(1000);
            });

            modelBuilder.Entity<VehicleRegisterMobileModel>(entity =>
            {
                entity.HasKey(e => e.VehicleRegisterMobileId)
                    .HasName("VehicleRegisterMobileModel_PK");

                entity.Property(e => e.VehicleRegisterMobileId).ValueGeneratedNever();

                entity.Property(e => e.Assets).HasMaxLength(4000);

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.CungDuongName).HasMaxLength(512);

                entity.Property(e => e.DriverIdCard).HasMaxLength(50);

                entity.Property(e => e.DriverName).HasMaxLength(200);

                entity.Property(e => e.Dvvc)
                    .HasColumnName("DVVC")
                    .HasMaxLength(255);

                entity.Property(e => e.Dvvccode)
                    .HasColumnName("DVVCCode")
                    .HasMaxLength(255);

                entity.Property(e => e.GiaoNhan).HasMaxLength(10);

                entity.Property(e => e.ModifyTime).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(4000);

                entity.Property(e => e.RegisterTime).HasColumnType("datetime");

                entity.Property(e => e.Romooc).HasMaxLength(50);

                entity.Property(e => e.ScaleTicketCode).HasMaxLength(50);

                entity.Property(e => e.SoDonHang).HasMaxLength(255);

                entity.Property(e => e.TapChat).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.ThoiGianToiDuKien).HasColumnType("datetime");

                entity.Property(e => e.ThoiGianToiThucTe).HasColumnType("datetime");

                entity.Property(e => e.TrongLuongGiaoDuKien).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TrongLuongGiaoThucTe).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);
            });

            modelBuilder.Entity<VehicleRegisterPodetailModel>(entity =>
            {
                entity.HasKey(e => e.VehicleRegisterPodetailId)
                    .HasName("VehicleRegisterPODetailModel_PK");

                entity.ToTable("VehicleRegisterPODetailModel");

                entity.Property(e => e.VehicleRegisterPodetailId)
                    .HasColumnName("VehicleRegisterPODetailId")
                    .ValueGeneratedNever();

                entity.Property(e => e.Poline)
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.TiLe).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.TrongLuong).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Unit)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VehicleVehicleOwnerMapping>(entity =>
            {
                entity.HasKey(e => new { e.VehicleId, e.VehicleOwner });

                entity.ToTable("Vehicle_VehicleOwner_Mapping");

                entity.Property(e => e.VehicleOwner).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
