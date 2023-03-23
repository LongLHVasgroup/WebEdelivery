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
    public class OrderMappingRepository : BaseRepository<OrderMapping>, IOrderMappingRepository
    {
        public OrderMappingRepository(WebContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<OrderMapping>> GetMappingNotDoneByPO(string poNumber)
        {
            IQueryable<OrderMapping> query = _dbContext.Set<OrderMapping>();
            query = query.AsNoTracking();
            query = query.Where(p=> p.OrderNumber == poNumber);
            query = query.Where(p => p.IsDone != true);
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<OrderMapping>> GetPONotDone(List<string> lstPOFull)
        {
            IQueryable<OrderMapping> query = _dbContext.Set<OrderMapping>();
            query = query.AsNoTracking();
            query = query.Where(p => lstPOFull.Contains(p.OrderNumber));
            query = query.Where(p => p.IsDone != true).OrderBy(p => p.OrderNumber);
            return await query.ToListAsync();
        }
    }
}
