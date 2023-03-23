using Microsoft.EntityFrameworkCore;
using RemoveVehicleRegisterOutDate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate.Services
{
    public class AppDbContext : DbContext
    {
        public DbSet<VehicleRegisterMobileModel> VehicleRegisterMobileModel { get; set; }

        public DbSet<VehicleRegisterPODetailModel> VehicleRegisterPODetailModel { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }


    }
}
