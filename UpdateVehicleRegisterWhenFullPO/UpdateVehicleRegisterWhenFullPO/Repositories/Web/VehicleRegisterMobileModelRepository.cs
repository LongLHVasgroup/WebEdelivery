using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdateVehicleRegisterWhenFullPO.Context;
using UpdateVehicleRegisterWhenFullPO.Contracts.Web;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

namespace UpdateVehicleRegisterWhenFullPO.Repositories.Web
{
    public class VehicleRegisterMobileModelRepository : BaseRepository<VehicleRegisterMobileModel>, IVehicleRegisterMobileModelRepository
    {
        public VehicleRegisterMobileModelRepository(WebContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<VehicleRegisterMobileModel>> GetAllowEditByPO(string poNumber)
        {
            IQueryable<VehicleRegisterMobileModel> query = _dbContext.Set<VehicleRegisterMobileModel>();
            query = query.AsNoTracking();
            query = query.Where(p => p.SoDonHang == poNumber);
            query = query.Where(p => p.AllowEdit == true);
            return await query.ToListAsync();
        }
    }
}
