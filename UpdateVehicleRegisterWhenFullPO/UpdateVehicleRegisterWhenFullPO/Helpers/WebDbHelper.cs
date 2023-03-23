using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateVehicleRegisterWhenFullPO.Context;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

namespace UpdateVehicleRegisterWhenFullPO.Helpers
{
    public class WebDbHelper
    {
        /*
        private WebContext dbContext;

        private DbContextOptions<WebContext> GetAllOptions()
        {
            var optionBuilder = new DbContextOptionsBuilder<WebContext>();
            optionBuilder.UseSqlServer(AppSetting.WebConnectionString);
            return optionBuilder.Options;
        }


        //GetAllUser
        public async Task<IEnumerable<OrderMapping>> GetAllOrderMapping()
        {
            using (dbContext = new WebContext(GetAllOptions()))
            {
                var users = await dbContext.OrderMapping.ToListAsync();
                return users;
            }
        }

        public async Task<VehicleRegisterMobileModel> AddAsync(VehicleRegisterMobileModel entity)
        {
            using (dbContext = new WebContext(GetAllOptions()))
            {
                dbContext.Set<VehicleRegisterMobileModel>().Add(entity);
                await dbContext.SaveChangesAsync();
                return entity;
            }

        }

        public async Task UpdateAsync(VehicleRegisterMobileModel entity)
        {
            using (dbContext = new WebContext(GetAllOptions()))
            {
                dbContext.Entry(entity).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(VehicleRegisterMobileModel entity)
        {
            using (dbContext = new WebContext(GetAllOptions()))
            {
                dbContext.Set<VehicleRegisterMobileModel>().Remove(entity);
                await dbContext.SaveChangesAsync();
            }
        }*/

    }
}
