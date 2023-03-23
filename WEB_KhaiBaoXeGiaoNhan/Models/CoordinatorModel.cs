using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class CoordinatorModel
    {
        public List<VehicleModel> Vehicle { get; set; }
        public ProviderModel VehicleOwner { get; set; }
    }
}