using Microsoft.EntityFrameworkCore;
using RemoveVehicleRegisterOutDate.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveVehicleRegisterOutDate.Services
{
    public class DbHelper
    {
        private AppDbContext dbContext;

        private DbContextOptions<AppDbContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionBuilder.UseSqlServer(AppSetting.ConnectionString);// Connection string
            return optionBuilder.Options;
        }


        // Lấy danh sách đăng ký đã hết hạn
        public List<VehicleRegisterMobileModel> GetOutOfDateList()
        {
            using(dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    // Xóa những dòng đăng ký hết hạng trong tuần trước
                    var listOutOfDate = dbContext.VehicleRegisterMobileModel
                        .Where(e => e.ThoiGianToiDuKien < DateTime.Today.AddDays(-6))
                        .Where(e => e.ModifyTime == null)
                        .Where(e => e.ScaleTicketCode == null)
                        .ToList();

                    if (listOutOfDate != null)
                        return listOutOfDate;
                    else
                        return new List<VehicleRegisterMobileModel>();
                }
                catch(Exception)
                {
                    throw;
                }
                
            }
        }
        public void RemoveRegisterOutOfDate( Guid id)
        {
            using (dbContext = new AppDbContext(GetAllOptions()))
            {
                try
                {
                    // Xóa ở bảng đăng ký
                    var listRegisterOutOfDate = dbContext.VehicleRegisterMobileModel
                       .Where(e => e.VehicleRegisterMobileId == id)
                       .First();

                    dbContext.VehicleRegisterMobileModel.Remove(listRegisterOutOfDate);

                    // Xóa ở bản Details
                    var listRegisterPOOutOfDate = dbContext.VehicleRegisterPODetailModel
                        .Where(e => e.VehicleRegisterMobileId == id)
                        .ToList();

                    dbContext.VehicleRegisterPODetailModel.RemoveRange(listRegisterPOOutOfDate);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }

            }

        }
    }
}
