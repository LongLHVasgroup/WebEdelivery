using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using UpdateVehicleRegisterWhenFullPO.Models;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace UpdateVehicleRegisterWhenFullPO.Context
{
    public partial class WebContext : DbContext
    {
        public WebContext()
        {
        }

        public WebContext(DbContextOptions<WebContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.getInstance().connWeb);
            }
        }

        public virtual DbSet<OrderMapping> OrderMapping { get; set; }
        public virtual DbSet<VehicleRegisterMobileModel> VehicleRegisterMobileModel { get; set; }
        public virtual DbSet<VehicleRegisterPodetailModel> VehicleRegisterPodetailModel { get; set; }
        public virtual DbSet<PolineModel> PolineModel { get; set; }
        public virtual DbSet<PomasterModel> PomasterModel { get; set; }
        public virtual DbSet<RegisterForShip> RegisterForShip { get; set; }
    }
}