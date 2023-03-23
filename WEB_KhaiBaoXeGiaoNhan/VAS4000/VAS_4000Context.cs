using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Models.Common;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WEB_KhaiBaoXeGiaoNhan.WebModelsPMC
{
    public partial class Vas_4000Context : DbContext
    {
        public Vas_4000Context()
        {
        }

        public Vas_4000Context(DbContextOptions<Vas_4000Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AutoNumber> AutoNumber { get; set; }
        public virtual DbSet<BargeModel> BargeModel { get; set; }
        public virtual DbSet<CheckIpPoSoModel> CheckIpPoSoModel { get; set; }
        public virtual DbSet<CheckingScrapModel> CheckingScrapModel { get; set; }
        public virtual DbSet<CompanyModel> CompanyModel { get; set; }
        public virtual DbSet<ConfigModel> ConfigModel { get; set; }
        public virtual DbSet<CungDuongModel> CungDuongModel { get; set; }
        public virtual DbSet<CustomerCungDuongMapping> CustomerCungDuongMapping { get; set; }
        public virtual DbSet<CustomerModel> CustomerModel { get; set; }
        public virtual DbSet<Domodel> Domodel { get; set; }
        public virtual DbSet<GateModel> GateModel { get; set; }
        public virtual DbSet<LogsTest> LogsTest { get; set; }
        public virtual DbSet<PolineModel> PolineModel { get; set; }
        public virtual DbSet<PomasterModel> PomasterModel { get; set; }
        public virtual DbSet<ProductAttributeModel> ProductAttributeModel { get; set; }
        public virtual DbSet<ProductConvertModel> ProductConvertModel { get; set; }
        public virtual DbSet<ProductModel> ProductModel { get; set; }
        public virtual DbSet<ProviderModel> ProviderModel { get; set; }
        public virtual DbSet<RoleModel> RoleModel { get; set; }
        public virtual DbSet<ScaleTicketHistoryModel> ScaleTicketHistoryModel { get; set; }
        public virtual DbSet<ScaleTicketMobileHistoryModel> ScaleTicketMobileHistoryModel { get; set; }
        public virtual DbSet<ScaleTicketMobileModel> ScaleTicketMobileModel { get; set; }
        public virtual DbSet<ScaleTicketModel> ScaleTicketModel { get; set; }
        public virtual DbSet<ScaleTicketPodetailMobileModel> ScaleTicketPodetailMobileModel { get; set; }
        public virtual DbSet<ScaleTicketPodetailModel> ScaleTicketPodetailModel { get; set; }
        public virtual DbSet<ScaleTicketPomodel> ScaleTicketPomodel { get; set; }
        public virtual DbSet<ScaleTicketSoDomasterMapping> ScaleTicketSoDomasterMapping { get; set; }
        public virtual DbSet<ScaleTicketSodetailModel> ScaleTicketSodetailModel { get; set; }
        public virtual DbSet<ScaleTicketSomodel> ScaleTicketSomodel { get; set; }
        public virtual DbSet<ScaleTicketTrmodel> ScaleTicketTrmodel { get; set; }
        public virtual DbSet<ScaleTicketTypeModel> ScaleTicketTypeModel { get; set; }
        public virtual DbSet<SolineModel> SolineModel { get; set; }
        public virtual DbSet<SomasterHistoryModel> SomasterHistoryModel { get; set; }
        public virtual DbSet<SomasterModel> SomasterModel { get; set; }
        public virtual DbSet<SyncPotoSapmodel> SyncPotoSapmodel { get; set; }
        public virtual DbSet<TestModel> TestModel { get; set; }
        public virtual DbSet<UnitModel> UnitModel { get; set; }
        public virtual DbSet<UpdateLogModel> UpdateLogModel { get; set; }
        public virtual DbSet<UserModel> UserModel { get; set; }
        public virtual DbSet<VehicleModel> VehicleModel { get; set; }
        public virtual DbSet<VehicleOwnerCungDuongMapping> VehicleOwnerCungDuongMapping { get; set; }
        public virtual DbSet<VehicleOwnerModel> VehicleOwnerModel { get; set; }
        public virtual DbSet<VehicleRegisterMobileModel> VehicleRegisterMobileModel { get; set; }
        public virtual DbSet<VehicleRegisterPodetailModel> VehicleRegisterPodetailModel { get; set; }
        public virtual DbSet<VehicleTypeModel> VehicleTypeModel { get; set; }
        public virtual DbSet<VehicleVehicleOwnerMapping> VehicleVehicleOwnerMapping { get; set; }
        public virtual DbSet<WarehouseModel> WarehouseModel { get; set; }
        public virtual DbSet<WeightScaleModel> WeightScaleModel { get; set; }
        public virtual DbSet<ZTestTableModel> ZTestTableModel { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.getInstance().connPMC);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AutoNumber>(entity =>
            {
                entity.HasKey(e => e.Code);

                entity.Property(e => e.Code).HasMaxLength(50);
            });

            modelBuilder.Entity<BargeModel>(entity =>
            {
                entity.HasKey(e => e.BargeId);

                entity.Property(e => e.BargeId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BargeNumber).HasMaxLength(50);

                entity.Property(e => e.BargeOwner).HasMaxLength(1000);
            });

            modelBuilder.Entity<CheckIpPoSoModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ApproveDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Ipstatus).HasColumnName("IPStatus");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.TmpDate).HasColumnType("datetime");

                entity.Property(e => e.TmpNote).HasMaxLength(500);

                entity.Property(e => e.UserApprove).HasMaxLength(100);

                entity.Property(e => e.UserCheck).HasMaxLength(100);
            });

            modelBuilder.Entity<CheckingScrapModel>(entity =>
            {
                entity.HasKey(e => e.CheckingScrapId)
                    .HasName("PK_ChekingScrapModel");

                entity.Property(e => e.CheckingScrapId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CheckingTime).HasColumnType("datetime");

                entity.Property(e => e.ChekingScrapIdInt).ValueGeneratedOnAdd();

                entity.Property(e => e.DriverIdCard).HasMaxLength(50);

                entity.Property(e => e.DriverName).HasMaxLength(200);

                entity.Property(e => e.GiaoNhan).HasMaxLength(10);

                entity.Property(e => e.InGateId).HasMaxLength(50);

                entity.Property(e => e.InHourGuard).HasColumnType("datetime");

                entity.Property(e => e.Note1).HasMaxLength(4000);

                entity.Property(e => e.Note3).HasMaxLength(4000);

                entity.Property(e => e.OutGateId).HasMaxLength(50);

                entity.Property(e => e.OutHourGuard).HasColumnType("datetime");

                entity.Property(e => e.ProviderCode).HasMaxLength(50);

                entity.Property(e => e.ProviderName).HasMaxLength(200);

                entity.Property(e => e.ReceiveType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Rfid)
                    .HasColumnName("RFID")
                    .HasMaxLength(100);

                entity.Property(e => e.Romooc).HasMaxLength(50);

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);

                entity.Property(e => e.VerifyTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<CompanyModel>(entity =>
            {
                entity.HasKey(e => e.CompanyCode);

                entity.Property(e => e.CompanyCode).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.CompanyName).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(50);
            });

            modelBuilder.Entity<ConfigModel>(entity =>
            {
                entity.HasKey(e => e.ConfigCode);

                entity.Property(e => e.ConfigCode).HasMaxLength(10);

                entity.Property(e => e.ConfigValueDecimal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ConfigValueString).HasMaxLength(50);
            });

            modelBuilder.Entity<CungDuongModel>(entity =>
            {
                entity.HasKey(e => e.CungDuongCode);

                entity.Property(e => e.CungDuongCode).ValueGeneratedNever();

                entity.Property(e => e.CungDuongName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.KhoangCach).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Plant)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CustomerCungDuongMapping>(entity =>
            {
                entity.HasKey(e => new { e.CustomerCode, e.CungDuongCode });

                entity.ToTable("Customer_CungDuong_Mapping");

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.IsDefault).HasColumnName("isDefault");
            });

            modelBuilder.Entity<CustomerModel>(entity =>
            {
                entity.HasKey(e => e.CustomerId);

                entity.Property(e => e.CustomerId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.Address).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.CungDuongName).HasMaxLength(500);

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Domodel>(entity =>
            {
                entity.HasKey(e => e.Donumber);

                entity.ToTable("DOModel");

                entity.Property(e => e.Donumber)
                    .HasColumnName("DONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.IsCompleted).HasColumnName("isCompleted");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoCuonBo).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Unit).HasMaxLength(3);
            });

            modelBuilder.Entity<GateModel>(entity =>
            {
                entity.HasKey(e => e.GateCode);

                entity.Property(e => e.GateCode).HasMaxLength(50);

                entity.Property(e => e.GateName).HasMaxLength(50);
            });

            modelBuilder.Entity<LogsTest>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Function).HasMaxLength(500);

                entity.Property(e => e.Input).HasMaxLength(100);
            });

            modelBuilder.Entity<PolineModel>(entity =>
            {
                entity.HasKey(e => new { e.Ponumber, e.Poline })
                    .HasName("PK_POLineModel_1");

                entity.ToTable("POLineModel");

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Poline)
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.CompanyCode).HasMaxLength(100);

                entity.Property(e => e.DeliveryDate).HasColumnType("date");

                entity.Property(e => e.DocumentDate).HasColumnType("date");

                entity.Property(e => e.IsDeliveryCompleted).HasColumnName("isDeliveryCompleted");

                entity.Property(e => e.IsPmccompleted).HasColumnName("isPMCCompleted");

                entity.Property(e => e.IsRelease).HasColumnName("isRelease");

                entity.Property(e => e.IsUnlimited).HasColumnName("isUnlimited");

                entity.Property(e => e.OverTolerance).HasColumnType("decimal(3, 1)");

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
                entity.HasKey(e => e.Ponumber);

                entity.ToTable("POMasterModel");

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.IsCompelete).HasColumnName("isCompelete");

                entity.Property(e => e.IsNhapKhau).HasColumnName("isNhapKhau");

                entity.Property(e => e.Note).HasMaxLength(500);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);

                entity.Property(e => e.QtyTotal).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaNhap).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<ProductAttributeModel>(entity =>
            {
                entity.HasKey(e => e.ProductAttributeId);

                entity.Property(e => e.ProductAttributeId).ValueGeneratedNever();

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.MaxSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ProductConvertModel>(entity =>
            {
                entity.HasKey(e => e.ProductConvertId);

                entity.Property(e => e.ProductConvertId).ValueGeneratedNever();

                entity.Property(e => e.ProductCodeFrom).HasMaxLength(100);

                entity.Property(e => e.ProductCodeTo).HasMaxLength(100);

                entity.Property(e => e.WarehouseCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.HasKey(e => e.ProductId);

                entity.Property(e => e.ProductId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.IsShowMobile).HasColumnName("isShowMobile");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.MaxSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.UnitCode).HasMaxLength(50);
            });

            modelBuilder.Entity<ProviderModel>(entity =>
            {
                entity.HasKey(e => e.ProviderId);

                entity.Property(e => e.ProviderId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.AccountGroup).HasMaxLength(50);

                entity.Property(e => e.Address).HasMaxLength(4000);

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);
            });

            modelBuilder.Entity<RoleModel>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.RoleCode).HasMaxLength(50);

                entity.Property(e => e.RoleName).HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketHistoryModel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketMobileHistoryModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketMobileHistoryId);

                entity.Property(e => e.ScaleTicketMobileHistoryId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.HistoryType).HasMaxLength(50);
            });

            modelBuilder.Entity<ScaleTicketMobileModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketMobileId);

                entity.Property(e => e.ScaleTicketMobileId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.BargeNumber).HasMaxLength(50);

                entity.Property(e => e.ContainerCount).HasMaxLength(50);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Is20Feet).HasColumnName("is20Feet");

                entity.Property(e => e.Is40Feet).HasColumnName("is40Feet");

                entity.Property(e => e.IsXeNoiBo).HasColumnName("isXeNoiBo");

                entity.Property(e => e.KgReduced).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.PercentReduced).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ScaleTicketCode).HasMaxLength(50);

                entity.Property(e => e.SoHieuCont1).HasMaxLength(100);

                entity.Property(e => e.SoHieuCont2).HasMaxLength(100);

                entity.Property(e => e.TrailersNumber).HasMaxLength(50);

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);

                entity.Property(e => e.VehicleTypeCode).HasMaxLength(10);
            });

            modelBuilder.Entity<ScaleTicketModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketId);

                entity.Property(e => e.ScaleTicketId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.ActualWeight).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ActualWeightAfterReduction).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.BargeNumber).HasMaxLength(50);

                entity.Property(e => e.ContainerCount).HasMaxLength(50);

                entity.Property(e => e.CungDuongName).HasMaxLength(100);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.FirstWeight).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.InHour).HasColumnType("datetime");

                entity.Property(e => e.InvalidSapmessage)
                    .HasColumnName("InvalidSAPMessage")
                    .HasMaxLength(100);

                entity.Property(e => e.Is20Feet).HasColumnName("is20Feet");

                entity.Property(e => e.Is40Feet).HasColumnName("is40Feet");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.IsDonTrongMax).HasColumnName("isDonTrongMax");

                entity.Property(e => e.IsDonTrongMin).HasColumnName("isDonTrongMin");

                entity.Property(e => e.IsInvalidSap).HasColumnName("isInvalidSAP");

                entity.Property(e => e.IsQuaTai).HasColumnName("isQuaTai");

                entity.Property(e => e.IsSendToSapcompleted).HasColumnName("isSendToSAPCompleted");

                entity.Property(e => e.IsTest).HasColumnName("isTest");

                entity.Property(e => e.IsVuotTyLeOd).HasColumnName("isVuotTyLeOD");

                entity.Property(e => e.IsXeNoiBo).HasColumnName("isXeNoiBo");

                entity.Property(e => e.KgReduced).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.OutHour).HasColumnType("datetime");

                entity.Property(e => e.PercentReduced).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ScaleTicketCode).HasMaxLength(50);

                entity.Property(e => e.ScaleTicketTypeCode).HasMaxLength(10);

                entity.Property(e => e.SecondWeight).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoHieuCont1).HasMaxLength(100);

                entity.Property(e => e.SoHieuCont2).HasMaxLength(100);

                entity.Property(e => e.SoftCode).HasMaxLength(100);

                entity.Property(e => e.StatusIp).HasColumnName("StatusIP");

                entity.Property(e => e.TotalReduced).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TrailersNumber).HasMaxLength(50);

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);

                entity.Property(e => e.VehicleOwner).HasMaxLength(50);

                entity.Property(e => e.VehicleOwnerName).HasMaxLength(100);

                entity.Property(e => e.VehicleTypeCode).HasMaxLength(10);
            });

            modelBuilder.Entity<ScaleTicketPodetailMobileModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketPodetailMobileId);

                entity.ToTable("ScaleTicketPODetailMobileModel");

                entity.Property(e => e.ScaleTicketPodetailMobileId)
                    .HasColumnName("ScaleTicketPODetailMobileId")
                    .HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.IsNoPo).HasColumnName("isNoPO");

                entity.Property(e => e.IsSendToSapcompleted).HasColumnName("isSendToSAPCompleted");

                entity.Property(e => e.Poline)
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Poqty)
                    .HasColumnName("POQty")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.Qty1).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Qty2).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaNhap).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TapChat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TyLeTrongLuong).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);

                entity.Property(e => e.Unit1).HasMaxLength(50);

                entity.Property(e => e.Unit2).HasMaxLength(50);
            });

            modelBuilder.Entity<ScaleTicketPodetailModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketPodetailId);

                entity.ToTable("ScaleTicketPODetailModel");

                entity.Property(e => e.ScaleTicketPodetailId)
                    .HasColumnName("ScaleTicketPODetailId")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsNoPo).HasColumnName("isNoPO");

                entity.Property(e => e.IsSendToSapcompleted).HasColumnName("isSendToSAPCompleted");

                entity.Property(e => e.Poline)
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Poqty)
                    .HasColumnName("POQty")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.Qty1).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Qty2).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaNhap).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TapChat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TyLeTrongLuong).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);

                entity.Property(e => e.Unit1).HasMaxLength(50);

                entity.Property(e => e.Unit2).HasMaxLength(50);
            });

            modelBuilder.Entity<ScaleTicketPomodel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketPoid);

                entity.ToTable("ScaleTicketPOModel");

                entity.Property(e => e.ScaleTicketPoid)
                    .HasColumnName("ScaleTicketPOId")
                    .ValueGeneratedNever();

                entity.Property(e => e.IsHasPo).HasColumnName("isHasPO");

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);

                entity.Property(e => e.SoLuongYcconLaiPo)
                    .HasColumnName("SoLuongYCConLaiPO")
                    .HasColumnType("decimal(19, 3)");

                entity.Property(e => e.WarehouseEntry).HasMaxLength(100);

                entity.Property(e => e.WarehouseEntryCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketSoDomasterMapping>(entity =>
            {
                entity.HasKey(e => new { e.ScaleTicketId, e.Donumber });

                entity.ToTable("ScaleTicketSO_DOMaster_Mapping");

                entity.Property(e => e.Donumber)
                    .HasColumnName("DONumber")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketSodetailModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketSodetailId);

                entity.ToTable("ScaleTicketSODetailModel");

                entity.Property(e => e.ScaleTicketSodetailId)
                    .HasColumnName("ScaleTicketSODetailId")
                    .ValueGeneratedNever();

                entity.Property(e => e.ActualSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.IsNoSo).HasColumnName("isNoSO");

                entity.Property(e => e.MaxSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.MinSingleWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.Qty1).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Qty2).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.SoLuongDaXuat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Soline)
                    .HasColumnName("SOLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Soqty)
                    .HasColumnName("SOQty")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);

                entity.Property(e => e.Unit1).HasMaxLength(50);

                entity.Property(e => e.Unit2).HasMaxLength(50);
            });

            modelBuilder.Entity<ScaleTicketSomodel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketSoid);

                entity.ToTable("ScaleTicketSOModel");

                entity.Property(e => e.ScaleTicketSoid)
                    .HasColumnName("ScaleTicketSOId")
                    .ValueGeneratedNever();

                entity.Property(e => e.ChenhLech).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.Donumber)
                    .HasColumnName("DONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.DonumberOther)
                    .HasColumnName("DONumberOther")
                    .HasMaxLength(100);

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.SoPhieuKho).HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.SonumberOrther)
                    .HasColumnName("SONumberOrther")
                    .HasMaxLength(100);

                entity.Property(e => e.TrongLuongOd)
                    .HasColumnName("TrongLuongOD")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.WarehouseExport).HasMaxLength(100);

                entity.Property(e => e.WarehouseExportCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketTrmodel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketTrid);

                entity.ToTable("ScaleTicketTRModel");

                entity.Property(e => e.ScaleTicketTrid)
                    .HasColumnName("ScaleTicketTRId")
                    .ValueGeneratedNever();

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.Qty1).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Qty2).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Unit1).HasMaxLength(50);

                entity.Property(e => e.Unit2).HasMaxLength(50);

                entity.Property(e => e.WarehouseEntry).HasMaxLength(100);

                entity.Property(e => e.WarehouseEntryCode).HasMaxLength(100);

                entity.Property(e => e.WarehouseExport).HasMaxLength(100);

                entity.Property(e => e.WarehouseExportCode).HasMaxLength(100);
            });

            modelBuilder.Entity<ScaleTicketTypeModel>(entity =>
            {
                entity.HasKey(e => e.ScaleTicketTypeCode);

                entity.Property(e => e.ScaleTicketTypeCode).HasMaxLength(10);

                entity.Property(e => e.ScaleTicketTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<SolineModel>(entity =>
            {
                entity.HasKey(e => new { e.Soline, e.Sonumber });

                entity.ToTable("SOLineModel");

                entity.Property(e => e.Soline)
                    .HasColumnName("SOLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

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

                entity.Property(e => e.UnderTolerance).HasColumnType("decimal(3, 1)");

                entity.Property(e => e.Unit)
                    .HasColumnName("UNIT")
                    .HasMaxLength(3);
            });

            modelBuilder.Entity<SomasterHistoryModel>(entity =>
            {
                entity.ToTable("SOMasterHistoryModel");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedUser)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Sonumber)
                    .IsRequired()
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<SomasterModel>(entity =>
            {
                entity.HasKey(e => e.Sonumber);

                entity.ToTable("SOMasterModel");

                entity.Property(e => e.Sonumber)
                    .HasColumnName("SONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.CustomerName).HasMaxLength(1000);

                entity.Property(e => e.IsCompelete).HasColumnName("isCompelete");

                entity.Property(e => e.IsNhapKhau).HasColumnName("isNhapKhau");

                entity.Property(e => e.QtyTotal).HasColumnType("decimal(13, 3)");

                entity.Property(e => e.SoLuongDaXuat).HasColumnType("decimal(18, 3)");
            });

            modelBuilder.Entity<SyncPotoSapmodel>(entity =>
            {
                entity.ToTable("SyncPOToSAPModel");

                entity.Property(e => e.BargeNumber).HasMaxLength(50);

                entity.Property(e => e.ContainerCount).HasMaxLength(50);

                entity.Property(e => e.InHour).HasColumnType("datetime");

                entity.Property(e => e.InvalidSapmessage)
                    .HasColumnName("InvalidSAPMessage")
                    .HasMaxLength(500);

                entity.Property(e => e.IsInvalidSap).HasColumnName("isInvalidSAP");

                entity.Property(e => e.IsSendToSapcompleted).HasColumnName("isSendToSAPCompleted");

                entity.Property(e => e.Poline)
                    .HasColumnName("POLine")
                    .HasMaxLength(100);

                entity.Property(e => e.Ponumber)
                    .HasColumnName("PONumber")
                    .HasMaxLength(100);

                entity.Property(e => e.Poqty)
                    .HasColumnName("POQty")
                    .HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ProductCode).HasMaxLength(100);

                entity.Property(e => e.ProductName).HasMaxLength(1000);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.ProviderName).HasMaxLength(1000);

                entity.Property(e => e.Qty1).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.Qty2).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.ScaleTicketCode).HasMaxLength(50);

                entity.Property(e => e.TapChat).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.TrailersNumber).HasMaxLength(50);

                entity.Property(e => e.Unit2).HasMaxLength(50);

                entity.Property(e => e.VehicleNumber).HasMaxLength(50);

                entity.Property(e => e.WarehouseEntryCode).HasMaxLength(100);
            });

            modelBuilder.Entity<TestModel>(entity =>
            {
                entity.HasKey(e => e.TestId);

                entity.Property(e => e.TestId).ValueGeneratedNever();

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Note).HasMaxLength(4000);
            });

            modelBuilder.Entity<UnitModel>(entity =>
            {
                entity.HasKey(e => e.UnitId);

                entity.Property(e => e.UnitCode)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UnitName).HasMaxLength(100);
            });

            modelBuilder.Entity<UpdateLogModel>(entity =>
            {
                entity.HasKey(e => e.UpdateId)
                    .HasName("PK_Table_1");

                entity.Property(e => e.UpdateId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CompanyCode).HasMaxLength(50);

                entity.Property(e => e.FunctionName).HasMaxLength(50);

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasMaxLength(50);

                entity.Property(e => e.Note).HasMaxLength(255);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.TimeReceive).HasMaxLength(50);

                entity.Property(e => e.TimeSend).HasMaxLength(50);

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.Property(e => e.Zkey)
                    .HasColumnName("ZKEY")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<UserModel>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.DeviceToken).HasMaxLength(255);

                entity.Property(e => e.FullName).HasMaxLength(100);

                entity.Property(e => e.GroupUser).HasMaxLength(255);

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.PasswordEnscrypt).HasMaxLength(50);

                entity.Property(e => e.RoldCode).HasMaxLength(50);

                entity.Property(e => e.UserName).HasMaxLength(100);
            });

            modelBuilder.Entity<VehicleModel>(entity =>
            {
                entity.HasKey(e => e.VehicleId);

                entity.Property(e => e.VehicleId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

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

            modelBuilder.Entity<VehicleOwnerCungDuongMapping>(entity =>
            {
                entity.HasKey(e => new { e.VehicleOwner, e.CungDuongCode });

                entity.ToTable("VehicleOwner_CungDuong_Mapping");

                entity.Property(e => e.VehicleOwner).HasMaxLength(100);

                entity.Property(e => e.IsDefault).HasColumnName("isDefault");
            });

            modelBuilder.Entity<VehicleOwnerModel>(entity =>
            {
                entity.HasKey(e => e.VehicleOwner);

                entity.Property(e => e.VehicleOwner).HasMaxLength(100);

                entity.Property(e => e.CustomerCode).HasMaxLength(100);

                entity.Property(e => e.ProviderCode).HasMaxLength(100);

                entity.Property(e => e.VehicleOwnerName).HasMaxLength(1000);
            });

            modelBuilder.Entity<VehicleRegisterMobileModel>(entity =>
            {
                entity.HasKey(e => e.VehicleRegisterMobileId);

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

                entity.Property(e => e.ScaleTicketCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

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
                entity.HasKey(e => e.VehicleRegisterPodetailId);

                entity.ToTable("VehicleRegisterPODetailModel");

                entity.Property(e => e.VehicleRegisterPodetailId)
                    .HasColumnName("VehicleRegisterPODetailId")
                    .HasDefaultValueSql("(newsequentialid())");

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

            modelBuilder.Entity<VehicleTypeModel>(entity =>
            {
                entity.HasKey(e => e.VehicleTypeCode);

                entity.Property(e => e.VehicleTypeCode).HasMaxLength(10);

                entity.Property(e => e.VehicleTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<VehicleVehicleOwnerMapping>(entity =>
            {
                entity.HasKey(e => new { e.VehicleId, e.VehicleOwner });

                entity.ToTable("Vehicle_VehicleOwner_Mapping");

                entity.Property(e => e.VehicleOwner).HasMaxLength(100);
            });

            modelBuilder.Entity<WarehouseModel>(entity =>
            {
                entity.HasKey(e => e.WarehouseId)
                    .HasName("PK_WarehouseModel_1");

                entity.Property(e => e.WarehouseId).HasDefaultValueSql("(newsequentialid())");

                entity.Property(e => e.CreatedTime).HasColumnType("datetime");

                entity.Property(e => e.IsSapdata).HasColumnName("isSAPData");

                entity.Property(e => e.LastEditedTime).HasColumnType("datetime");

                entity.Property(e => e.WarehouseCode)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.WarehouseName).HasMaxLength(1000);
            });

            modelBuilder.Entity<WeightScaleModel>(entity =>
            {
                entity.HasKey(e => e.SoftCode);

                entity.Property(e => e.SoftCode).HasMaxLength(50);

                entity.Property(e => e.CompanyCode).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.SoftName).HasMaxLength(500);
            });

            modelBuilder.Entity<ZTestTableModel>(entity =>
            {
                entity.ToTable("Z_TestTableModel");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.LastRun).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}