using System;
using System.Collections.Generic;
using WEB_KhaiBaoXeGiaoNhan.Models;

namespace WEB_KhaiBaoXeGiaoNhan
{
    public class VehicleInfoRegister
    {
        public int id { get; set; }

        public string BSX { get; set; }

        public string TaiXe { get; set; }

        public string PersonalID { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime InHour { get; set; }

        public List<MaterialModel> Materials { get; set; }

        public int? TapChat { get; set; }

        public int? TrongLuongHang { get; set; }
    }
}