using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UpdateVehicleRegisterWhenFullPO.Context
{
    public partial class BanCanContext : DbContext
    {
        public BanCanContext()
        {
        }

        public BanCanContext(DbContextOptions<BanCanContext> options)
            : base(options)
        {
        }

        public virtual DbSet<VehicleRegisterMobileModel> VehicleRegisterMobileModel { get; set; }
        public virtual DbSet<VehicleRegisterPodetailModel> VehicleRegisterPodetailModel { get; set; }


        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
