using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UpdateVehicleRegisterWhenFullPO.Models.WebModels;

namespace UpdateVehicleRegisterWhenFullPO.Contracts.Web
{
    public interface IVehicleRegisterMobileModelRepository : IRepositoryBase<VehicleRegisterMobileModel>
    {
        Task<IEnumerable<VehicleRegisterMobileModel>> GetAllowEditByPO(string poNumber);
    }
}
