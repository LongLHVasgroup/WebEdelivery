using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

namespace UpdateVehicleRegisterWhenFullPO.Contracts.Web
{
    public interface IOrderMappingRepository : IRepositoryBase<OrderMapping>
    {
        Task<IEnumerable<OrderMapping>> GetMappingNotDoneByPO(string poNumber);
        Task<IEnumerable<OrderMapping>> GetPONotDone(List<string> lstPOFull);
    }
}
