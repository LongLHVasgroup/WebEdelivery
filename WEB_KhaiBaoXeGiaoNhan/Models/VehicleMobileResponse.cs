using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB_KhaiBaoXeGiaoNhan.WebModels;

namespace WEB_KhaiBaoXeGiaoNhan.Models
{
    public class VehicleMobileResponse
    {
        private VehicleRegisterMobileModel item;
        private List<VehicleRegisterPodetailModel> detail;
        private DriverRegister driver;
        private List<PolineModel> polines;
        private bool isQuaTai;
        private decimal? weightAllowed;
        public VehicleRegisterMobileModel Item { get => item; set => item = value; }
        public List<VehicleRegisterPodetailModel> Detail { get => detail; set => detail = value; }
        public DriverRegister Driver { get => driver; set => driver = value; }
        public List<PolineModel> Polines { get => polines; set => polines = value; }
        public bool IsQuaTai { get => isQuaTai; set => isQuaTai = value; }
        public decimal? WeightAllowed { get => weightAllowed; set => weightAllowed = value; }
    }
}